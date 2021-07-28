using OpenCvSharp;

namespace SampleSegmenter.Models
{
    public class ImageFromFile
    {
        private readonly string _fileName;

        private readonly Mat imageMat;

        public ImageFromFile(string fileName)
        {
            _fileName = fileName;
            imageMat = new Mat(_fileName, ImreadModes.AnyDepth | ImreadModes.AnyColor);
            Cv2.CvtColor(imageMat, imageMat, ColorConversionCodes.BGR2BGRA);
        }

        public Mat GetImageMat()
        {
            return imageMat.Clone();
        }

        public string GetFileName()
        {
            return _fileName;
        }
    }
}
