using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Autodesk.Navisworks.Api;

using SpeckleCore;
using SpeckleCoreGeometryClasses;
using SpeckleNavisworks.Models;

namespace SpeckleNavisworks.ViewModels
{
    public class StreamDetails : Base
    {
        #region Fields and Properties
        private SpeckleStreamWrapper _speckleStreamWrapper;
        private SpeckleApiClient _speckleApiClient;
        private List<Autodesk.Navisworks.Api.SelectionSet> _selectionSets;
        private Autodesk.Navisworks.Api.SelectionSet _selectedSelectionSet;
        private bool _pushCommandCanExecute;

        public SpeckleStreamWrapper SpeckleStreamWrapper
        {
            get
            {
                return _speckleStreamWrapper;
            }
            set
            {
                _speckleStreamWrapper = value;
            }
        }

        public SpeckleApiClient SpeckleApiClient
        {
            get
            {
                return _speckleApiClient;
            }
            set
            {
                _speckleApiClient = value;
            }
        }

        public List<Autodesk.Navisworks.Api.SelectionSet> SelectionSets
        {
            get
            {
                return _selectionSets;
            }
            set
            {
                _selectionSets = value;
                OnPropertyChanged();
            }
        }

        public Autodesk.Navisworks.Api.SelectionSet SelectedSelectionSet
        {
            get
            {
                return _selectedSelectionSet;
            }
            set
            {
                _selectedSelectionSet = value;
                SpeckleStreamWrapper.SelectedSelectionSet = value.DisplayName;
                OnPropertyChanged();
            }
        }

        public bool PushCommandCanExecute
        {
            get
            {
                return _pushCommandCanExecute;
            }
            set
            {
                _pushCommandCanExecute = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        private ICommand _pushCommand;

        public ICommand PushCommand
        {
            get
            {
                if (_pushCommand == null)
                {
                    _pushCommand = new RelayCommand(
                        p => PushCommandCanExecute,
                        p => PushData());
                }

                return _pushCommand;
            }
        }
        #endregion

        public StreamDetails()
        {
            PushCommandCanExecute = true;
            SelectionSets = new List<Autodesk.Navisworks.Api.SelectionSet>(Models.NavisworksWrapper.GetAllSearchSets());
        }

        public async void PushData()
        {
            PushCommandCanExecute = false;

            ModelItemCollection modelItems = SelectedSelectionSet.GetSelectedItems();

            NavisworksWrapper.GetGeometryData(modelItems);

            List<SpeckleObject> speckleObjects = new List<SpeckleObject>();
            foreach (var mesh in NavisworksWrapper.Meshes)
            {
                var props = new Dictionary<string, object>();
                props.Add("Name", mesh.Name);
                var speckleMesh = new SpeckleMesh(mesh.CreateVerticesArray(), mesh.CreateVertexIndexArray(), new int[] { }, new double[] { }, properties: props);
                speckleMesh.Name = mesh.Name;
                speckleObjects.Add(speckleMesh);
            }

            // Empty meshes list
            NavisworksWrapper.Reset();

            StreamController.Client.Stream = SpeckleStreamWrapper.SpeckleStream;
            StreamController.UpdateStream(speckleObjects);

            //await SpeckleStreamWrapper.UpdateStream(
            //    Models.StreamController.Client,
            //    speckleMeshes
            //    .Cast<object>()
            //    .ToList());

            //PushCommandCanExecute = await SpeckleStreamWrapper.UpdateStream(
            //    Models.StreamController.Client,
            //    Helpers.Geometry.GetBoundingBoxCenter(modelItems)
            //    .Cast<Object>()
            //    .ToList());
        }
    }
}
