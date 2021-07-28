using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace SampleSegmenter.Converters
{
    public class ImageConverter
    {
        public static BitmapImage Convert(Mat image)
        {
            Bitmap bitmap = BitmapConverter.ToBitmap(image);

            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            BitmapImage result = new();
            result.BeginInit();
            result.CacheOption = BitmapCacheOption.OnLoad;
            result.StreamSource = ms;
            result.EndInit();
            result.Freeze();
            return result;
        }
    }
}
