using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitServices;
using RevitServices.Transactions;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Structure.OOP
{
    class ObjectTransaction
    {
        #region Properties
        public static UIDocument uidoc { get; set; }

        #endregion

        #region Method COnstructor
        public ObjectTransaction(UIDocument _uidoc)
        {
            uidoc = _uidoc;
        }
        #endregion

        public object Start(object input)
        {
            TransactionManager.Instance.EnsureInTransaction(uidoc.Document);
            return input;
        }
        public object End(object input)
        {
            TransactionManager.Instance.ForceCloseTransaction();
            uidoc.RefreshActiveView();
            return input;
        }
    }
}
