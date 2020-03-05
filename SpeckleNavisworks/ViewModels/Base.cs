using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeckleNavisworks.ViewModels
{
    public abstract class Base : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region Control of ViewModels
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

        /// <summary>
        /// With this method you can go back to the previous ViewModel
        /// </summary>
        /// <param name="newViewModel"></param>
        public void ChangeViewModel(ViewModels.Base newViewModel)
        {
            SelectedViewModel = newViewModel;
        }
        #endregion
    }
}
