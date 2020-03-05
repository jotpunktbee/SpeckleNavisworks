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
            ChangeViewModel(new ViewModels.Overview());
        }

        private void ChangeToNewStream()
        {
            ChangeViewModel(new ViewModels.NewStream(this));
        }
    }
}
