using OpenCvSharp;
using SampleSegmenter.Enums;
using System.Collections.Generic;
using System.Windows.Media;

namespace SampleSegmenter.Interfaces
{
    public interface IImageProcessingService
    {
        public ImageSource Image { get; set; }
        public ImageProcessingSteps SelectedImageProcessingStep { get; set; }
        public string Information { get; set; }
        public bool IsImageLoaded { get; set; }
        public void SetOrigMat(Mat orig);
        public List<IContourInfo> GetContoursInfo();
        public void SetOptions<T>(T options);
    }
}
