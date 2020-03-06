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
}
