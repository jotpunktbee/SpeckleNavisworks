using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Navisworks.Api;
using SpeckleNavisworks.Models;

namespace SpeckleNavisworks.Helpers
{
    public static class Geometry
    {
        /// <summary>
        /// Get center points of multiple bounding boxes
        /// </summary>
        /// <param name="modelItems"></param>
        /// <returns></returns>
        public static List<Point3D> GetBoundingBoxCenter(ModelItemCollection modelItems)
        {
            var result = new List<Point3D>();

            foreach (ModelItem modelItem in modelItems)
            {
                result.Add(GetBoundingBoxCenter(modelItem));
            }

            return result;
        }

        /// <summary>
        /// Get center point of bounding box
        /// </summary>
        /// <param name="modelItem"></param>
        /// <returns></returns>
        public static Point3D GetBoundingBoxCenter(ModelItem modelItem)
        {
            var boundingBox = modelItem.BoundingBox();
            var minPoint = boundingBox.Min;
            var maxPoint = boundingBox.Max;

            return new Point3D(
                (minPoint.X + maxPoint.X) / 2,
                (minPoint.Y + maxPoint.Y) / 2,
                (minPoint.Z + maxPoint.Z) / 2);
        }

        public static float[] OfPoint(float x, float y, float z)
        {
            float w = (Convert.ToSingle(NavisworksWrapper.Elements[3]) * x) + (Convert.ToSingle(NavisworksWrapper.Elements[7]) * y) + (Convert.ToSingle(NavisworksWrapper.Elements[11]) * z) + Convert.ToSingle(NavisworksWrapper.Elements[15]);

            return new float[]
            {
                (Convert.ToSingle(NavisworksWrapper.Elements[0]) * x + Convert.ToSingle(NavisworksWrapper.Elements[4]) * y + Convert.ToSingle(NavisworksWrapper.Elements[8]) * z + Convert.ToSingle(NavisworksWrapper.Elements[12])) / w,
                (Convert.ToSingle(NavisworksWrapper.Elements[1]) * x + Convert.ToSingle(NavisworksWrapper.Elements[5]) * y + Convert.ToSingle(NavisworksWrapper.Elements[9]) * z + Convert.ToSingle(NavisworksWrapper.Elements[13])) / w,
                (Convert.ToSingle(NavisworksWrapper.Elements[2]) * x + Convert.ToSingle(NavisworksWrapper.Elements[6]) * y + Convert.ToSingle(NavisworksWrapper.Elements[10]) * z + Convert.ToSingle(NavisworksWrapper.Elements[14])) / w
            };
        }

        public static Point3D OfPoint3D(Point3D pt)
        {
            double w = (NavisworksWrapper.Elements[3] * pt.X) + (NavisworksWrapper.Elements[7] * pt.Y) + (NavisworksWrapper.Elements[11] * pt.Z) + NavisworksWrapper.Elements[15];

            return new Point3D(
                ((NavisworksWrapper.Elements[0] * pt.X) + (NavisworksWrapper.Elements[4] * pt.Y) + (NavisworksWrapper.Elements[8] * pt.Z) + NavisworksWrapper.Elements[12]) / w,
                ((NavisworksWrapper.Elements[1] * pt.X) + (NavisworksWrapper.Elements[5] * pt.Y) + (NavisworksWrapper.Elements[9] * pt.Z) + NavisworksWrapper.Elements[13]) / w,
                ((NavisworksWrapper.Elements[2] * pt.X) + (NavisworksWrapper.Elements[6] * pt.Y) + (NavisworksWrapper.Elements[10] * pt.Z) + NavisworksWrapper.Elements[14]) / w);
        }
    }
}
