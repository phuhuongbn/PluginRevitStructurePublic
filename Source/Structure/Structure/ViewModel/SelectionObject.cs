
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using dl = DSCore.List;
using ds = DSCore.String;

namespace Structure.ViewModel
{
    
    class SelectionFilter : ISelectionFilter
    {
        #region Properties
        public List<string> categories { get; set; }
        #endregion

        #region Constructor
        public SelectionFilter(List<string> _categories)
        {
            categories = _categories;
        }
        #endregion

        public bool AllowElement(Element elem)
        {
            if (dl.Contains(categories, elem.Category.Name) == true) { return true; }
            else { return false; }
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }

    class SelectionObject
    {
        public Element PickObjectByCategory(List<string> categories, string message)
        {
            Element element = null;
            SelectionFilter selectionFilter = new SelectionFilter(categories);
            try
            {
                Reference select = VarGlobal.uidoc.Selection.PickObject(ObjectType.Element, selectionFilter, message);
                element = VarGlobal.doc.GetElement(select);
            }
            catch { }
            return element;
        }
    }


}
