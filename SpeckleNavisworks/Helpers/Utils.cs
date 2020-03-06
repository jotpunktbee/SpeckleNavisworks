using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeckleNavisworks.Helpers
{
    public static class Utils
    {
        // https://forums.autodesk.com/t5/navisworks-api/geometry-in-wrong-location-when-accessing-in-api/m-p/8912249#M4611
        public static T[] ToArray<T>(Array arr)
        {
            T[] result = new T[arr.Length];
            Array.Copy(arr, result, result.Length);

            return result;
        }
    }
}
