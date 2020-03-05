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

        public static List<string> GetAllSearchSets()
        {
            List<string> result = new List<string>();
            var selectionSets = Document.SelectionSets;
            var savedItemCollection = selectionSets.Value;

            foreach (var saveItem in savedItemCollection)
            {
                if (saveItem.IsGroup)
                {
                    result.Add(saveItem.DisplayName);
                }
            }

            return result;
        }
    }
}
