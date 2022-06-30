using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TourPlanner.Client.ViewModels
{
    internal class MapImageViewModel : BaseViewModel
    {
        public ImageSource? ImagePath { get; set; }
        public MapImageViewModel()
        {

        }

        public void LoadImage(Uri imageUri)
        {
            ImagePath = new BitmapImage(imageUri);
        }
    }
}
