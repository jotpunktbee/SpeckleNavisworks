using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpeckleNavisworks.ViewModels
{
    public class MainWindow : Base
    {
        private ViewModels.Base _selectedViewModel;

        public ViewModels.Base SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        private ICommand _newStreamCommand;

        public ICommand NewStreamCommand
        {
            get
            {
                if (_newStreamCommand == null)
                {
                    _newStreamCommand = new RelayCommand(
                        p => true,
                        p => ChangeToNewStream());
                }

                return _newStreamCommand;
            }
        }

        public MainWindow()
        {
            SelectedViewModel = new ViewModels.Overview();
        }

        private void ChangeToNewStream()
        {
            SelectedViewModel = new ViewModels.NewStream();
        }
    }
}
