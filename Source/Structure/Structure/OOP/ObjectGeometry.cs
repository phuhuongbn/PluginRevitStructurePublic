using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Structure.OOP
{
    class ObjectGeometry
    {
        public object Translate(object geometry, XYZ vector, double distance)
        {
            var vectorNormalize = vector.Normalize();
            var vectorTranslate = vectorNormalize * (distance / 304.8);
            var transform = Transform.CreateTranslation(vectorTranslate);
            object result = null;
            if (geometry.GetType() == typeof(Autodesk.Revit.DB.XYZ))
            {
                result = transform.OfPoint((XYZ)geometry);
            }
            else if (geometry.GetType() == typeof(Autodesk.Revit.DB.Line))
            {
                var geometryLine = geometry as Line;
                result = geometryLine.CreateTransformed(transform);
            }
            return result;
        }
    }
}
