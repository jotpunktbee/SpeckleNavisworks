using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

using SpeckleCore;

namespace SpeckleNavisworks
{
    [PluginAttribute(
        "SpeckleNavisworks",
        "C5590956-6D30-4B50-8105-E6E346F3140E",
        ToolTip = "Create new Speckle Stream",
        DisplayName = "SpeckleInNavisworks")]
    public class CreateNewSpeckleStream : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            MessageBox.Show("Hello World!");

            return 0;
        }
    }
}
