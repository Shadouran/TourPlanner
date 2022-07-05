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
using System.Windows.Shapes;
using TourPlanner.Client.ViewModels;

namespace TourPlanner.Client.Views
{
    /// <summary>
    /// Interaction logic for AddTourLogDialog.xaml
    /// </summary>
    public partial class AddTourLogDialog : Window
    {
        public AddTourLogDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is ICloseWindow cw)
            {
                cw.Close += () => Close();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
