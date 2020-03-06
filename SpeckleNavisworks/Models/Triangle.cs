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
}
