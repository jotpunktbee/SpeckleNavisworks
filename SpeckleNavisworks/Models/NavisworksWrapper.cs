using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Navisworks.Api;

namespace SpeckleNavisworks.Models
{
    public static class NavisworksWrapper
    {
        public static Document Document;
        public static string DocumentGUID;

        public static List<SelectionSet> GetAllSearchSets()
        {
            List<SelectionSet> result = new List<SelectionSet>();
            var selectionSets = Document.SelectionSets;
            var savedItemCollection = selectionSets.Value;

            foreach (var saveItem in savedItemCollection)
            {
                if (!saveItem.IsGroup)
                {
                    SelectionSet selectionSet = saveItem as SelectionSet;

                    if (selectionSet != null)
                    {
                        result.Add(selectionSet);
                    }
                }
            }

            return result;
        }

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
    }
}
