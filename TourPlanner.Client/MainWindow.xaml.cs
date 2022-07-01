using System.Windows;
using TourPlanner.Client.ViewModels;

namespace TourPlanner.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(DataContext is ICloseWindow cw)
            {
                e.Cancel = cw.OnClosing();
            }
        }
    }
}
