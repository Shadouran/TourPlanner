using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
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
        ImageSource? _image;
        public ImageSource? Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
        public void LoadImage(Uri? imageUri)
        {
            var bitmap = new BitmapImage();
            if (imageUri == null)
            {
                Image = bitmap;
                return;
            }
            // TODO pretty sure this violates layering
            using var stream = File.OpenRead(imageUri.AbsolutePath);
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            Image = bitmap;
        }
    }
}
