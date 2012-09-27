using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;

namespace Workout
{
    public class Utils
    {
        public static BitmapImage GetImage(string fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string path = System.IO.Path.Combine("Media", fileName);

                if (!store.FileExists(path)) return null;

                using (var stream = store.OpenFile(path, FileMode.Open))
                {
                    var image = new BitmapImage();
                    image.SetSource(stream);
                    return image;
                }
            }
        }
    }
}
