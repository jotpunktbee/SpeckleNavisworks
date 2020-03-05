using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpeckleCore;

namespace SpeckleNavisworks.ViewModels
{
    public class StreamDetails : Base
    {
        private SpeckleStream _speckleStream;
        private SpeckleApiClient _speckleApiClient;
        private List<Autodesk.Navisworks.Api.SelectionSet> _selectionSets;

        public SpeckleStream SpeckleStream
        {
            get
            {
                return _speckleStream;
            }
            set
            {
                _speckleStream = value;
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

        public StreamDetails()
        {
            SelectionSets = new List<Autodesk.Navisworks.Api.SelectionSet>(Models.NavisworksWrapper.GetAllSearchSets());
        }
    }
}
