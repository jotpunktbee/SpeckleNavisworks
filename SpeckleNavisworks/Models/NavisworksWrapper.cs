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
    }
}
