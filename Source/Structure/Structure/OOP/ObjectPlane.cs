using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Structure.OOP;

namespace Structure.OOP
{
    class ObjectPlane
    {
        public Plane ByThreePoints(XYZ point1, XYZ point2, XYZ point3)
        {
            return Plane.CreateByThreePoints(point1, point2, point3);
        }
        public Plane ByOriginNormal(XYZ origin, XYZ normal)
        {
            return Plane.CreateByNormalAndOrigin(normal, origin);
        }
        public XYZ ProjectPointToPlane(XYZ p0,Plane plane)
        {
            var vectorProject = new ObjectVector().ByTwoPoints(p0, plane.Origin);
            var pointProject = p0 + plane.Normal * vectorProject.DotProduct(plane.Normal);
            return pointProject;
        }
    }
}
