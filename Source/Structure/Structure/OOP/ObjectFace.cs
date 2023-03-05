using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Structure.ViewModel;
using Structure.Model;
using Structure.OOP;
using dl = DSCore.List;
using ds = DSCore.String;
using _List = System.Collections.IList;

namespace Structure.OOP
{
    class ObjectFace
    {
        public XYZ CenterPoint(PlanarFace face)
        {
            var points = PerimeterPoints(face);
            var lineMiddle = Line.CreateBound((XYZ)points[0], (XYZ)points[2]);
            var middlePoint = lineMiddle.Evaluate(0.5, true);

            return middlePoint;
        }
        public List<XYZ> PerimeterPoints(PlanarFace face)
        {
            var edges0 = PerimeterCurves(face);
            var edge1 = dl.FirstItem(edges0) as Curve;
            var edges1 = dl.RestOfItems(edges0);
            var startPoint = edge1.GetEndPoint(0);
            var endPoint = edge1.GetEndPoint(1);
            var points = new List<XYZ>();
            var edges2 = new List<Curve>();
            points.Add(startPoint);
            var index1 = 0;
            var i = 0;
            while (dl.Count(edges1) != 0 && i < 20)
            {
                foreach(Curve edge in edges1)
                {
                    var _startPoint = edge.GetEndPoint(0);
                    var _endPoint = edge.GetEndPoint(1);

                    if (_startPoint.DistanceTo(startPoint) < 1) { points.Add(_endPoint); }
                    else if (_endPoint.DistanceTo(startPoint) < 1) { points.Add(_startPoint); }
                    else { edges2.Add(edge); }
                }
                if (dl.Count(edges2) != 0)
                {
                    edges1 = edges2;
                    edges2 = new List<Curve>();
                    startPoint = dl.LastItem(points) as XYZ;
                }
                else
                {
                    points.Add(endPoint);
                    break;
                }
                i = i + 1;
            }
            return points;

            
        }
        public List<Curve> PerimeterCurves(PlanarFace face)
        {
            List<Curve> curves = new List<Curve>();
            foreach (EdgeArray x in face.EdgeLoops) 
            { 
                foreach(Edge y in x)
                {
                    curves.Add(y.AsCurve());
                }
                 
            }
            return curves;
        }
             
    }
}
