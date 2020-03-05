using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Interop.ComApi;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace SpeckleNavisworks.Models
{
    public class NavisworksCallbackGeometryListener : ComApi.InwSimplePrimitivesCB
    {
        public void Triangle(InwSimpleVertex v1, InwSimpleVertex v2, InwSimpleVertex v3)
        {
            SpeckleNavisworks.Models.Triangle triangle = new Triangle(
                SpeckleNavisworks.Models.Triangle.ConvertToPoint3D(v1),
                SpeckleNavisworks.Models.Triangle.ConvertToPoint3D(v2),
                SpeckleNavisworks.Models.Triangle.ConvertToPoint3D(v3));

            SpeckleNavisworks.Models.NavisworksWrapper.Mesh.Vertices.Add(triangle.Pt1);
            SpeckleNavisworks.Models.NavisworksWrapper.Mesh.Vertices.Add(triangle.Pt2);
            SpeckleNavisworks.Models.NavisworksWrapper.Mesh.Vertices.Add(triangle.Pt3);

            SpeckleNavisworks.Models.NavisworksWrapper.Mesh.Triangles.Add(triangle);
        }

        public void Line(InwSimpleVertex v1, InwSimpleVertex v2)
        {
            throw new NotImplementedException();
        }

        public void Point(InwSimpleVertex v1)
        {
            throw new NotImplementedException();
        }

        public void SnapPoint(InwSimpleVertex v1)
        {
            throw new NotImplementedException();
        }
    }
}
