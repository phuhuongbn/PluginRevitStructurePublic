using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Structure.ViewModel;
using Structure.Model;
using Structure.OOP;
using dl = DSCore.List;
using ds = DSCore.String;
using RevitServices.Transactions;
using Autodesk.Revit.DB.Structure;
using wf = System.Windows.Forms;

namespace Structure.Model
{
    class PutRebarOnColumn : IExternalEventHandler
    {
        // pick column
        public void Execute(UIApplication app)
        {
            try
            {

                #region Get object Document
                UIApplication uiapp = app;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;
                #endregion

                TransactionManager.SetupManager();
                ObjectTransaction tran = new ObjectTransaction(uidoc);
                #region Transaction Group

                var tg = new TransactionGroup(doc);
                tg.Start("Put Rebar On Column");

                #endregion

                #region Method put rebar on column

                SelectionObject selectionObject = new SelectionObject();
                Element column = selectionObject.PickObjectByCategory(new List<string> { "Structural Columns" }, "Please Select a column !!!");

                GeometryInstance geometryInstance = column.get_Geometry(new Options()).Cast<GeometryInstance>().First();
                Solid solidColumn = geometryInstance.GetInstanceGeometry()
                                    .Cast<Solid>()
                                    .FirstOrDefault(v => v.Volume != 0);
                var surfaces = solidColumn.Faces;

                #region Get center point of surface
                List<XYZ> origins = new List<XYZ>();
                foreach (var face in surfaces)
                {
                    var middlePoint = new ObjectFace().CenterPoint((PlanarFace)face);
                    origins.Add(middlePoint);
                }
                #region Find point lowest and point highest
                List<double> coorZ = (from x in origins select x.Z).ToList();
                List<object> originsObject = new List<object>();
                foreach (var x in origins) { object _object = x as object; originsObject.Add(_object); }
                var sort1 = new ObjectList().SortByKeys(originsObject, coorZ);
                var pointLowest = dl.FirstItem(sort1.Item1);
                var pointHighest = dl.LastItem(sort1.Item1);
                // vector from pointLowest - pointHightest
                var vec0 = new ObjectVector().ByTwoPoints((XYZ)pointLowest, (XYZ)pointHighest);
                // offset perimeterCurves of face lowest
                List<object> surfacesObject = new List<object>();
                foreach (var x in surfaces) { object _object = x as object; surfacesObject.Add(_object); }
                var faceLowest = new ObjectList().SortByKeys(surfacesObject, coorZ).Item1[0];
                var perimeterPoints = new ObjectFace().PerimeterPoints((PlanarFace)faceLowest);
                var perimeterCurves = new ObjectFace().PerimeterCurves((PlanarFace)faceLowest);
                var linesRebarMain = new List<Curve>();
                var linesRebarBranch = new List<Curve>();
                var perimeterPointsOffsetBranch = new List<XYZ>();

                var diameterRebarBranch = VarGlobal.RebarBranchTyle.Parameters
                                        .Cast<Parameter>()
                                        .Where(x => x.Definition.Name == "Bar Diameter")
                                        .FirstOrDefault().AsDouble() * 304.8;
                var diameterRebarMain = VarGlobal.RebarMainTyle.Parameters
                                        .Cast<Parameter>()
                                        .Where(x => x.Definition.Name == "Bar Diameter")
                                        .FirstOrDefault().AsDouble() * 304.8;
                var b = diameterRebarBranch / 2 + VarGlobal.DistanceConcreteProtect;
                var c = b + diameterRebarMain;
                var _perimeterCurvesOffsetBranch = new List<Curve>();
                var _perimeterCurvesOffsetMain = new List<Curve>();
                foreach (var _curve in perimeterCurves)
                {
                    var _pointMiddle = new ObjectLine().PointAtParameter((Line)_curve, 0.5);
                    var vec1 = new ObjectVector().ByTwoPoints(_pointMiddle, (XYZ)pointLowest);
                    var _curveTranslateBranch = new ObjectGeometry().Translate(_curve, vec1, b);
                    var _curveTranslateMain = new ObjectGeometry().Translate(_curve, vec1, c);
                    _perimeterCurvesOffsetBranch.Add((Curve)_curveTranslateBranch);
                    _perimeterCurvesOffsetMain.Add((Curve)_curveTranslateMain);
                }
                var _perimeterCurvesOffsetBranch1 = dl.ShiftIndices(_perimeterCurvesOffsetBranch, -1);
                var _perimeterCurvesOffsetMain1 = dl.ShiftIndices(_perimeterCurvesOffsetMain, -1);
                var index = 0;
                foreach (var x in _perimeterCurvesOffsetBranch)
                {
                    var _pointBranch = new ObjectLine().Intersect((Line)_perimeterCurvesOffsetBranch[index], (Line)_perimeterCurvesOffsetBranch1[index]);
                    var _pointMain = new ObjectLine().Intersect((Line)_perimeterCurvesOffsetMain[index], (Line)_perimeterCurvesOffsetMain1[index]);
                    var _lineRebarMain = new ObjectLine().ByStartPointAndDirectionLength((XYZ)_pointMain, vec0, new ObjectVector().Length(vec0));
                    linesRebarMain.Add(_lineRebarMain);
                    perimeterPointsOffsetBranch.Add((XYZ)_pointBranch);
                    index = index + 1;
                }
                index = 0;
                var perimeterPointsOffsetBranch1 = dl.ShiftIndices(perimeterPointsOffsetBranch, -1);
                foreach (var _pointBranch in perimeterPointsOffsetBranch)
                {
                    var _lineRebarBranch = new ObjectLine().ByStartPointEndPoint(_pointBranch, (XYZ)perimeterPointsOffsetBranch1[index]);
                    linesRebarBranch.Add(_lineRebarBranch);
                    index = index + 1;
                }

                var _rebarStyleMain = VarGlobal.RebarBranchStyle;
                var _rebarTypeMain = VarGlobal.RebarMainTyle;
                object _startHookTyleMain = null;
                var _endHookTyleMain = _startHookTyleMain;
                var _hostMain = column;
                var _vectorMain = new ObjectVector().Normal(vec0);
                var _curvesMain = linesRebarMain;
                var _startHookOrientationMain = "Left";
                var _endHookOrientationMain = "Left";

                var _rebarStyleBranch = VarGlobal.RebarBranchStyle;
                var _rebarTypeBranch = VarGlobal.RebarBranchTyle;
                object _startHookTyleBranch = VarGlobal.RebarBranchStartHookTyle;
                var _endHookTyleBranch = VarGlobal.RebarBranchEndHookTyle;
                var _hostBranch = column;
                var _vectorBranch = vec0;
                var _curvesBranch = linesRebarBranch;
                var _startHookOrientationBranch = VarGlobal.RebarBranchStartOri;
                var _endHookOrientationBranch = VarGlobal.RebarBranchEndOri;


                var tran11 = tran.Start(_hostMain);

                TransactionManager.Instance.EnsureInTransaction(doc);
                var rebarsMain = new List<Rebar>();
                foreach (var _curve in _curvesMain)
                {
                    var rebarMain = new ObjectRebar().CreateFromCurve(_rebarStyleMain, (RebarBarType)_rebarTypeMain, (RebarHookType)_startHookTyleMain, (RebarHookType)_endHookTyleMain, (Element)tran11, _vectorMain, _curve, _startHookOrientationMain, _endHookOrientationMain);
                    rebarsMain.Add(rebarMain);
                }
                var rebarBranch = new ObjectRebar().CreateFromCurves(_rebarStyleBranch, (RebarBarType)_rebarTypeBranch, (RebarHookType)_startHookTyleBranch, (RebarHookType)_endHookTyleBranch, (Element)tran11, _vectorBranch, _curvesBranch, _startHookOrientationBranch, _endHookOrientationBranch);
                TransactionManager.Instance.TransactionTaskDone();

                var tran12 = tran.End(new Tuple<List<Rebar>, Rebar>(rebarsMain, rebarBranch));
                var tran13 = tran.Start(tran12) as Tuple<List<Rebar>, Rebar>;
                var _rebarBranch = tran13.Item2;
                var _spacing = 150;
                var _arrayLength = new ObjectVector().Length(vec0);
                var _barsOnNormalSide = true;
                var _includeFirstBar = true;
                var _includeLastBar = true;

                TransactionManager.Instance.EnsureInTransaction(doc);
                var arrayRebarBranch = new ObjectRebar().SetLayoutAsMaximumSpacing((Rebar)_rebarBranch, _spacing, _arrayLength, _barsOnNormalSide, _includeFirstBar, _includeLastBar);
                TransactionManager.Instance.TransactionTaskDone();

                var tran14 = tran.End(tran13);
                var tran15 = tran.Start(tran14) as Tuple<List<Rebar>, Rebar>;

                var rebarsMain1 = tran15.Item1;
                var rebarBranch1 = tran15.Item2;
                var activeView = doc.ActiveView;

                if (activeView.GetType() == typeof(Autodesk.Revit.DB.View3D))
                {
                    TransactionManager.Instance.EnsureInTransaction(doc);
                    foreach (var _rebarMain in rebarsMain1) { _rebarMain.SetSolidInView((View3D)activeView, true); }
                    rebarBranch1.SetSolidInView((View3D)activeView, true);
                    TransactionManager.Instance.TransactionTaskDone();
                }
                var tran16 = tran.End(tran15);
                #endregion
                #endregion
                #endregion

                #region End Transaction Group

                tg.Assimilate();

                #endregion
            }
            catch (Exception ex)
            {

                wf.MessageBox.Show(ex.Message);
            }

        }

        public string GetName()
        {
            return "Put Rebar On Column";
        }
    }
}
