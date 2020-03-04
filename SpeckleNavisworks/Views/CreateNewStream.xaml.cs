using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeckleNavisworks.Views
{
    /// <summary>
    /// Interaktionslogik für CreateNewStream.xaml
    /// </summary>
    public partial class CreateNewStream : Window
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
            }
        }
        public ViewModels.Base ViewModel { get; set; }

        public CreateNewStream(ViewModels.Base viewModel)
        {
            InitializeComponent();

            SelectedViewModel = viewModel;

            //ViewModel = viewModel;
            //DataContext = ViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // (ViewModel as ViewModels.CreateNewStream).GetAllStreams();
        }
    }
}
