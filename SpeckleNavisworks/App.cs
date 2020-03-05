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
        "SpeckleCreateStreamNavisworks",
        "C5590956-6D30-4B50-8105-E6E346F3140E",
        ToolTip = "Connection to Speckle",
        DisplayName = "Speckle Navisworks")]
    public class CreateNewSpeckleStream : AddInPlugin
    {
        private static Account account;
        private static SpeckleApiClient client;

        public override int Execute(params string[] parameters)
        {
            MessageBox.Show("Hello World!");

            SpeckleCore.SpeckleInitializer.Initialize();

            try
            {
                account = LocalContext.GetDefaultAccount();
            }
            catch (Exception ex)
            {
                List<Account> accounts = LocalContext.GetAccountsByEmail("julian.bolliger@mum.ch");
                account = accounts.First();
            }

            var client = new SpeckleApiClient(account.RestApi, false, "Navisworks");
            client.AuthToken = account.Token;

            Models.StreamController.Client = client;

            var mainWindow = new Views.MainWindow();
            mainWindow.ShowDialog();

            //var createNewStreamViewModel = new ViewModels.CreateNewStream(client) { Account = account };
            //var createNewStreamView = new Views.CreateNewStream(createNewStreamViewModel);
            //createNewStreamView.ShowDialog();

            return 0;
        }
    }
}
