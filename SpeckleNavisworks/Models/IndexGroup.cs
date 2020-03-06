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
