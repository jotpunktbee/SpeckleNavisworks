using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MainWindow()
        {
            SelectedViewModel = new ViewModels.Overview();
        }
    }
}
