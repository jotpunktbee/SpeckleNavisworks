using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpeckleCore;

namespace SpeckleNavisworks.ViewModels
{
    public class NewStream : Base
    {
        private SpeckleApiClient _client;

        public SpeckleApiClient Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }

        public NewStream()
        {
        }

        public NewStream(SpeckleApiClient client)
        {
            Client = client;
        }
    }
}
