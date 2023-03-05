using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace Structure
{
    class VarGlobal
    {
        #region Document
        public static Document doc { get; set; }
        public static UIDocument uidoc { get; set; }
        #endregion

        #region Put rebar on column

        public static double DistanceConcreteProtect { get; set; }
        public static double Spacing { get; set; }
        public static RebarBarType RebarMainTyle { get; set; }
        public static string RebarBranchStyle { get; set; }
        public static RebarBarType RebarBranchTyle { get; set; }
        public static RebarHookType RebarBranchStartHookTyle { get; set; }
        public static RebarHookType RebarBranchEndHookTyle { get; set; }
        public static string RebarBranchStartOri { get; set; }
        public static string RebarBranchEndOri { get; set; }

        public static ExternalEvent EventPutRebarOnColumn { get; set; }
        #endregion
    }
}
