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

        public StreamDetails() { }
    }
}
