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
using RevitServices.Transactions;
using Autodesk.Revit.DB.Structure;

namespace Structure
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region Get object Document
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            #endregion

            #region Set var global

            VarGlobal.doc = doc;
            VarGlobal.uidoc = uidoc;
            VarGlobal.EventPutRebarOnColumn = ExternalEvent.Create(new PutRebarOnColumn());

            #endregion

            #region Call Main form

            MainForm mainForm = new MainForm();
            mainForm.Show();

            #endregion

            string test = "Hello";

            var a = 5;
            var b = 10;
            var c = a + b;

            return Result.Succeeded;
        }
    }
}
