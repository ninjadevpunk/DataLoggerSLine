using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3.Services
{
    /// <summary>
    /// Class for BitmapImage management and related tasks.
    /// </summary>
    public class BitmapService
    {

        public static string SaveResizedImage(string path)
        {
            try
            {
                var bitmap = new BitmapImage(new Uri(path));

                int size = 128;

                double scale = Math.Min(
                    (double)size / bitmap.PixelWidth,
                    (double)size / bitmap.PixelHeight
                );

                var resized = new TransformedBitmap(bitmap, new ScaleTransform(scale, scale));

                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger", "Profile Pics");
                Directory.CreateDirectory(folder);
                string newPath = Path.Combine(folder, Guid.NewGuid() + ".png");

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(resized));
                    encoder.Save(stream);
                }

                return newPath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static BitmapImage? LoadImage(string path)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
