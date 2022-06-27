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

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            if(DataContext is ICloseWindow cw)
            {
                cw.Close += () => Close();
            }
        }
    }
}
