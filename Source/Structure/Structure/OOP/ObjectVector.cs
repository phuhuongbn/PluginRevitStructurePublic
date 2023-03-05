using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Structure.OOP;

namespace Structure.OOP
{
    class ObjectVector
    {
        public XYZ ByTwoPoints(XYZ start, XYZ end)
        {
            XYZ vectorNormalize = (end - start).Normalize();

            return vectorNormalize.Multiply(start.DistanceTo(end));
        }
        public double Length(XYZ vector)
        {
            return vector.GetLength() * 304.8;
        }
        public double AngleBetween(XYZ vectorFirst, XYZ vectorSecond) 
        {
            return vectorFirst.AngleTo(vectorSecond) * 57.2957795;
        }
		
        public XYZ Normal(XYZ vector)
        {
            var Ox = new XYZ(1, 0, 0);
            var Oy = new XYZ(0, 1, 0);
            var Oz = new XYZ(0, 0, 1);
            if (((AngleBetween(vector, Ox) > -1) && (AngleBetween(vector, Ox) < 1)) || ((AngleBetween(vector, Ox) > 179) && (AngleBetween(vector, Ox) < 181))) { return Oy; }
            else if (((AngleBetween(vector, Oy) > -1) && (AngleBetween(vector, Oy) < 1)) || ((AngleBetween(vector, Oy) > 179) && (AngleBetween(vector, Oy) < 181))) { return Ox; }
            else if (((AngleBetween(vector, Oz) > -1) && (AngleBetween(vector, Oz) < 1)) || ((AngleBetween(vector, Oz) > 179) && (AngleBetween(vector, Oz) < 181))) { return Oy; }
            var Xoy = new ObjectPlane().ByOriginNormal(new XYZ(0, 0, 1), new XYZ(0, 0, 0));
            var pointProject = new ObjectPlane().ProjectPointToPlane(vector, Xoy);
            return new ObjectPlane().ByThreePoints(vector, Xoy.Origin, pointProject).Normal;
        }
    }
}
