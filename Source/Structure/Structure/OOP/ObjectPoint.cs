using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace myParent.OOP
{
    class ObjectPoint
    {
        public static XYZ ByCoordinates (float X, float Y, float Z)
        {
            return new XYZ(X / 304.8, Y / 304.8, Z / 304.8);
        }
    }
}
