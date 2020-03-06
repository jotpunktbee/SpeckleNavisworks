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
    }

    public class Mesh
    {
        public List<Triangle> Triangles = new List<Triangle>();
        public HashSet<Point3D> Vertices = new HashSet<Point3D>();
        public List<Point3D> VerticesList = new List<Point3D>();
        public string Name;

        public Mesh()
        {

        }

        public void CreateIndexGroups()
        {
            VerticesList = Vertices.ToList();

            foreach (var triangle in Triangles)
            {
                triangle.CreateIndexGroup(VerticesList);
            }
        }

        public int[] CreateVertexIndexArray()
        {
            var result = new List<int>();

            foreach (var triangle in Triangles)
            {
                result.AddRange(triangle.IndexGroup.ToArray());
            }

            return result.ToArray();
        }

        public double[] CreateVerticesArray()
        {
            var result = new List<double>();

            foreach (var vertex in VerticesList)
            {
                result.Add(vertex.X);
                result.Add(vertex.Y);
                result.Add(vertex.Z);
            }

            return result.ToArray();
        }
    }

    public class Triangle
    {
        public Point3D Pt1;
        public Point3D Pt2;
        public Point3D Pt3;
        public IndexGroup IndexGroup;

        public Triangle(Point3D pt1, Point3D pt2, Point3D pt3)
        {
            Pt1 = pt1;
            Pt2 = pt2;
            Pt3 = pt3;
        }

        public static Point3D ConvertToPoint3D(ComApi.InwSimpleVertex v)
        {
            Array vArray = (Array)(object)v.coord;

            return new Point3D(
                (float)vArray.GetValue(1), 
                (float)vArray.GetValue(2), 
                (float)vArray.GetValue(3));
        }

        public void CreateIndexGroup(List<Point3D> points)
        {
            IndexGroup = new IndexGroup(
                points.IndexOf(Pt1),
                points.IndexOf(Pt2),
                points.IndexOf(Pt3));
        }
    }

    public class IndexGroup
    {
        public int A;
        public int B;
        public int C;
        public int D;

        public IndexGroup() { }

        public IndexGroup(int a, int b, int c, int d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public IndexGroup(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
            D = -1;
        }

        public int[] ToArray()
        {
            int[] result = new int[4];
            result[0] = 0;
            result[1] = A;
            result[2] = B;
            result[3] = C;

            return result;
        }
    }
}
