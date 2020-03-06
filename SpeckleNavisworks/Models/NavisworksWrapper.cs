using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Navisworks.Api;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace SpeckleNavisworks.Models
{
    public static class NavisworksWrapper
    {
        public static Document Document;
        public static string DocumentGUID;
        public static ComBridge ComBridge;

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

        public static List<Mesh> Meshes = new List<Mesh>();
        public static Mesh Mesh;
        public static double[] Elements;

        public static void GetGeometryData(ModelItemCollection modelItems)
        {
            ComApi.InwOpSelection oSelection = ComBridge.ToInwOpSelection(modelItems);
            var navisworksCallbackGeometryListener = new NavisworksCallbackGeometryListener();

            foreach (ComApi.InwOaPath3 path in oSelection.Paths())
            {
                Mesh = new Mesh();

                foreach (ComApi.InwOaFragment3 fragment in path.Fragments())
                {
                    // Convert relative coordinates to absolute coordinates
                    ComApi.InwLTransform3f3 localToWorld = (ComApi.InwLTransform3f3)(object)fragment.GetLocalToWorldMatrix();
                    Array localToWorldMatrix = (Array)(object)localToWorld.Matrix;
                    Elements = Helpers.Utils.ToArray<double>(localToWorldMatrix);

                    fragment.GenerateSimplePrimitives(ComApi.nwEVertexProperty.eNORMAL, navisworksCallbackGeometryListener);
                }

                Mesh.Name = ComBridge.ToModelItem(path).DisplayName;
                Mesh.CreateIndexGroups();
                Meshes.Add(Mesh);
            }
        }

        public static void Reset()
        {
            Meshes = new List<Mesh>();
            Mesh = null;
            Elements = null;
        }
    }
}
