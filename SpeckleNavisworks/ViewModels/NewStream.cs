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
        #region Fields and Properties
        private string _streamName;
        private string _streamDescription;
        private ViewModels.Base _previousViewModel;

        public string StreamName
        {
            get
            {
                return _streamName;
            }
            set
            {
                _streamName = value;
                OnPropertyChanged();
            }
        }

        public string StreamDescription
        {
            get
            {
                return _streamDescription;
            }
            set
            {
                _streamDescription = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        private ICommand _createStreamCommand;

        public ICommand CreateStreamCommand
        {
            get
            {
                if (_createStreamCommand == null)
                {
                    _createStreamCommand = new RelayCommand(
                        p => CreateNewStreamCanExecute(),
                        p => CreateNewStream());
                }

                return _createStreamCommand;
            }
        }
        #endregion

        public NewStream() { } 

        public NewStream(ViewModels.Base previousViewModel)
        {
            _previousViewModel = previousViewModel;
        }

        #region Create new stream
        private async void CreateNewStream()
        {
            if (await Models.StreamController.NewStream(StreamName, StreamDescription))
            {
                _previousViewModel.ChangeViewModel(new ViewModels.Overview());
            }
        }

        private bool CreateNewStreamCanExecute()
        {
            if (String.IsNullOrWhiteSpace(StreamName))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
