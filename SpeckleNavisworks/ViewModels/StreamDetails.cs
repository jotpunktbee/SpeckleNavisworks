using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpeckleCore;
using SpeckleNavisworks.Models;

namespace SpeckleNavisworks.ViewModels
{
    public class StreamDetails : Base
    {
        private SpeckleStreamWrapper _speckleStreamWrapper;
        private SpeckleApiClient _speckleApiClient;
        private List<Autodesk.Navisworks.Api.SelectionSet> _selectionSets;
        private Autodesk.Navisworks.Api.SelectionSet _selectedSelectionSet;

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

        public StreamDetails()
        {
            SelectionSets = new List<Autodesk.Navisworks.Api.SelectionSet>(Models.NavisworksWrapper.GetAllSearchSets());
        }
    }
}
