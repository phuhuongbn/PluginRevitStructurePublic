using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Structure.OOP
{
    class ObjectLine
    {
        public Line ByStartPointEndPoint(XYZ start, XYZ end)
        {
            Line line = Line.CreateBound(start, end);

            return line;
        }
        public Line ByStartPointAndDirectionLength(XYZ start, XYZ direction, double length)
        {
            XYZ end = start +  direction.Normalize() * (length / 304.8);

            Line line = Line.CreateBound(start, end);

            

            return line;
        }
        public XYZ PointAtParameter(Line line, double param)
        {
            return line.Evaluate(param, true);
        }
        public object Intersect(Line lineFirst, Line lineSecond)
        {
            var line11 = lineFirst;
            var line12 = lineSecond;
            IntersectionResultArray intersections;
            var intersect = line11.Intersect(line12, out intersections);
            object result = null;
            if (intersect == SetComparisonResult.Disjoint) { return result; }
            var intersection = intersections.Cast<IntersectionResult>().First();
            result = intersection.XYZPoint;
            return result;
        }
    }
}
