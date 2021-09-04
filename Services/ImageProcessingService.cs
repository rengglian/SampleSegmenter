using OpenCvSharp;
using Prism.Mvvm;
using SampleSegmenter.Converters;
using SampleSegmenter.Enums;
using SampleSegmenter.Interfaces;
using SampleSegmenter.Models;
using SampleSegmenter.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SampleSegmenter.Services
{
    public class ImageProcessingService : BindableBase, IImageProcessingService
    {
        private Mat _orig;
        private Mat _denoised;
        private Mat _grayscaled;
        private Mat _binarized;
        private Mat _masked;
        private Mat _dilated;
        private Mat _result;

        private readonly List<ContourInfo> _contoursInfo;

        private EqualizerOptions _equalizerOptions;
        private DenoiseOptions _denoiseOptions;
        private MaskOptions _maskOptions;
        private ThresholdOptions _thresholdOptions;
        private DilateOptions _dilateOptions;
        private ContoursOptions _contoursOptions;

        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private ImageProcessingSteps _selectedImageProcessingStep = ImageProcessingSteps.Result;
        public ImageProcessingSteps SelectedImageProcessingStep
        {
            get { return _selectedImageProcessingStep; }
            set
            {
                SetProperty(ref _selectedImageProcessingStep, value);
                UpdateImage(SelectedImageProcessingStep);
            }
        }

        private string _information;
        public string Information
        {
            get { return _information; }
            set { SetProperty(ref _information, value); }
        }

        private bool _isImageLoaded;
        public bool IsImageLoaded
        {
            get { return _isImageLoaded; }
            set { SetProperty(ref _isImageLoaded, value); }
        }

        public ImageProcessingService()
        {
            _denoiseOptions = new();
            _equalizerOptions = new();
            _maskOptions = new();
            _thresholdOptions = new();
            _dilateOptions = new();
            _contoursOptions = new();
            _contoursInfo = new();
        }

        public void SetOrigMat(Mat orig)
        {
            Information = "Set Original Image";
            _orig = orig.Clone();
            Update();
        }

        public List<ContourInfo> GetContoursInfo()
        {
            return _contoursInfo;
        }

        public void SetOptions<T>(T options)
        {
            Type optionType = options.GetType();
            if (optionType == typeof(EqualizerOptions)) { _equalizerOptions = options as EqualizerOptions; }
            if (optionType == typeof(DenoiseOptions)) { _denoiseOptions = options as DenoiseOptions; }
            if (optionType == typeof(MaskOptions)) { _maskOptions = options as MaskOptions; }
            if (optionType == typeof(ThresholdOptions)) { _thresholdOptions = options as ThresholdOptions; }
            if (optionType == typeof(DilateOptions)) { _dilateOptions = options as DilateOptions; }
            if (optionType == typeof(ContoursOptions)) { _contoursOptions = options as ContoursOptions; }
            Update();
        }

        private void UpdateImage(ImageProcessingSteps imageProcessingSteps)
        {
            switch (imageProcessingSteps)
            {
                case ImageProcessingSteps.Orignal:
                    {
                        Image = ImageConverter.Convert(_orig.Clone());
                        break;
                    }
                case ImageProcessingSteps.Denoised:
                    {
                        Image = ImageConverter.Convert(_denoised.Clone());
                        break;
                    }
                case ImageProcessingSteps.Grayscaled:
                    {
                        Image = ImageConverter.Convert(_grayscaled.Clone());
                        break;
                    }
                case ImageProcessingSteps.Masked:
                    {
                        Image = ImageConverter.Convert(_masked.Clone());
                        break;
                    }
                case ImageProcessingSteps.Binarized:
                    {
                        Image = ImageConverter.Convert(_binarized.Clone());
                        break;
                    }
                case ImageProcessingSteps.Dilated:
                    {
                        Image = ImageConverter.Convert(_dilated.Clone());
                        break;
                    }
                case ImageProcessingSteps.Result:
                    {
                        Image = ImageConverter.Convert(_result.Clone());
                        break;
                    }
            }
        }

        private void Update()
        {
            Task.Factory.StartNew(() =>
            {
                Denoise();
                Grayscale();
                Mask();
                Threshold();
                Dilate();
                Contours();
                UpdateImage(SelectedImageProcessingStep);
            });
        }

        private void Denoise()
        {
            Information = "Denoise Image";
            _denoised = _orig.Clone();
            Cv2.FastNlMeansDenoisingColored(
                _orig, 
                _denoised, 
                _denoiseOptions.H, 
                _denoiseOptions.HColor, 
                _denoiseOptions.TemplateWindowSize, 
                _denoiseOptions.TemplateWindowSize);
        }

        private void Grayscale()
        {
            Information = "Grayscale Image";
            _grayscaled = _denoised.Clone();
            Cv2.CvtColor(_denoised, _grayscaled, ColorConversionCodes.BGR2GRAY);
            if (_equalizerOptions.IsEnabled) Cv2.EqualizeHist(_grayscaled, _grayscaled);
        }

        private void Mask()
        {
            Information = "Mask Image";
            using var mask = new Mat(_grayscaled.Height, _grayscaled.Width, MatType.CV_8UC1, new Scalar(0, 0, 0));
            using var destination = new Mat(_grayscaled.Height, _grayscaled.Width, MatType.CV_8UC1, new Scalar(0, 0, 0));

            if (_maskOptions.IsEnabled)
            {
                //Cv2.Circle(mask, _binarized.Width / 2, _binarized.Height/2, _binarized.Height/4, new Scalar(255), -1);
                Cv2.Rectangle(mask, new Point(_maskOptions.X, _maskOptions.Y), new Point(_maskOptions.X + _maskOptions.Width, _maskOptions.Y + _maskOptions.Height), new Scalar(255, 255, 255), -1);
            }
            else
            {
                Cv2.Rectangle(mask, new Point(2, 2), new Point(_grayscaled.Width-2, _grayscaled.Height-2), new Scalar(255, 255, 255), -1);
            }

            _grayscaled.CopyTo(destination, mask);
            _masked = destination.Clone();

        }

        private void Threshold()
        {
            Information = "Binarize Image";

            ThresholdTypes thresholdTypes = _thresholdOptions.Invert ? ThresholdTypes.BinaryInv : ThresholdTypes.Binary;
            if (_thresholdOptions.UseOtsu)
            {
                thresholdTypes |= ThresholdTypes.Otsu;
            }            

            _binarized = _masked.Clone();
            Cv2.Threshold(_masked, _binarized, _thresholdOptions.ThresholdValue, _thresholdOptions.MaxValue, thresholdTypes);
        }

        private void Dilate()
        {
            Information = "Dilate Image";
            _dilated = _binarized.Clone();

            if (_dilateOptions.IsEnabled)
            {
                var struct_element = Cv2.GetStructuringElement(
                    MorphShapes.Cross,
                    new Size(2 * _dilateOptions.Size + 1, 2 * _dilateOptions.Size + 1),
                    new Point(_dilateOptions.Size, _dilateOptions.Size));

                Cv2.Dilate(_binarized, _dilated, struct_element, iterations: _dilateOptions.Iterations);
            }
        }

        private void Contours()
        {
            Information = "Find Contours";
            _result = _orig.Clone();
            Cv2.FindContours(_dilated, out Point[][] contours, out HierarchyIndex[] hierarchyIndexes, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);

            List<Point2f> circlesCenter = new();

            _contoursInfo.Clear();

            if (_contoursOptions.Advanced)
            {
                circlesCenter = AdvancedAnalysis(contours);
                var avgPoint = new Point2f(_contoursInfo.Average(p => p.CircleX), _contoursInfo.Average(p => p.CircleY));
                var ordered = _contoursInfo.OrderBy(t => Math.Atan2(avgPoint.Y - t.CircleY, avgPoint.X - t.CircleX)).ToArray();

                var tessf = new Point2f( 1.2f, 2.3f );

                var test = _contoursInfo.SelectMany(o => new Point2f[]{ new Point2f ( o.CircleX, o.CircleY ) }).ToList();
                var summaryPoly = Cv2.ApproxPolyDP(test, 3, true);
                Cv2.MinEnclosingCircle(summaryPoly, out Point2f summaryCenter, out float summaryRadius);

                _contoursInfo.Add(new ContourInfo
                {
                    ContourName = "Summary",
                    CentroidX = 0,
                    CentroidY = 0,
                    CircleX = summaryCenter.X - _orig.Width / 2.0f,
                    CircleY = summaryCenter.Y - _orig.Height / 2.0f,
                    CircleRadius = summaryRadius,
                    ContourArea = 0.0,
                    ContourCircumference = 0.0,
                    HistogramValues = new()
                });
            }
            else
            {
                BasicAnalysis(contours, hierarchyIndexes);
            }


            IsImageLoaded = true;
            Information += " - Done";
        }

        private void BasicAnalysis(Point[][] contours, HierarchyIndex[] hierarchyIndexes)
        {
            var rand = new Random();
            var thickness = _contoursOptions.FillContours ? -1 : 2;
            
            int counter = 0;

            foreach (HierarchyIndex hi in hierarchyIndexes)
            {
                var contourIndex = hi.Next;

                if (hi.Next != -1 && hi.Parent == 0)
                {
                    var contour = contours[contourIndex];
                    if (_contoursOptions.ConvexHull)
                    {
                        contour = Cv2.ConvexHull(contour, true);
                        contours[contourIndex] = contour;
                    }
                    var area = Cv2.ContourArea(contour);
                    if (area > _contoursOptions.MinimumArea)
                    {
                        Information = @"Analyse Contour " + ++counter;
                        Moments moment = Cv2.Moments(contour);
                        var x = (int)(moment.M10 / moment.M00);
                        var y = (int)(moment.M01 / moment.M00);

                        double circumference = Cv2.ArcLength(contour, true);
                        var randomColor = new Scalar(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), 255);
                        Cv2.DrawContours(
                            _result,
                            contours,
                            contourIndex,
                            color: randomColor,
                            thickness: thickness,
                            lineType: LineTypes.Link8,
                            hierarchy: hierarchyIndexes,
                            maxLevel: int.MaxValue);

                        int[] channels = { 0 };
                        var hist = new Mat();
                        int[] hdims = { 256 };
                        Rangef[] ranges = { new Rangef(0, 256), }; // min/max 

                        using var hist_mask = new Mat(_grayscaled.Height, _grayscaled.Width, MatType.CV_8UC1, new Scalar(0, 0, 0));
                        Cv2.DrawContours(hist_mask, contours, contourIndex, new Scalar(255, 255, 255), thickness: -1);

                        _contoursInfo.Add(new ContourInfo
                        {
                            ContourName = contourIndex.ToString(),
                            CentroidX = x,
                            CentroidY = y,
                            CircleX = 0,
                            CircleY = 0,
                            CircleRadius = 0,
                            ContourArea = area,
                            ContourCircumference = circumference,
                            HistogramValues = new()
                        });
                    }
                }
            }
        }

        private List<Point2f> AdvancedAnalysis(Point[][] contours)
        {
            var rand = new Random();
            var thickness = _contoursOptions.FillContours ? -1 : 2;

            int counter = 0;

            var circlesCenter = new List<Point2f>();

            foreach (var contour in contours)
            {
                var area = Cv2.ContourArea(contour);
                if (area > _contoursOptions.MinimumArea)
                {
                    Information = @"Analyse Contour " + ++counter;
                    Moments moment = Cv2.Moments(contour);
                    var x = (int)(moment.M10 / moment.M00);
                    var y = (int)(moment.M01 / moment.M00);

                    double circumference = Cv2.ArcLength(contour, true);
                    var randomColor = new Scalar(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), 255);

                    Cv2.DrawContours(
                        _result,
                        new[] { contour },
                        0,
                        color: randomColor,
                        thickness: thickness,
                        lineType: LineTypes.Link8);

                    int[] channels = { 0 };
                    var hist = new Mat();
                    int[] hdims = { 256 };
                    Rangef[] ranges = { new Rangef(0, 256), }; // min/max 

                    using var hist_mask = new Mat(_grayscaled.Height, _grayscaled.Width, MatType.CV_8UC1, new Scalar(0, 0, 0));
                    Cv2.DrawContours(hist_mask, new[] { contour }, 0, new Scalar(255, 255, 255), thickness: -1);

                    Cv2.CalcHist(new Mat[] { _grayscaled }, channels, hist_mask, hist, 1, hdims, ranges);

                    List<float> tmpHistValues = new();

                    for (int j = 0; j < hdims[0]; ++j)
                    {
                        tmpHistValues.Add(hist.Get<float>(j));
                    }

                    var contourPoly = Cv2.ApproxPolyDP(contour, 3, true);
                    Cv2.MinEnclosingCircle(contourPoly, out Point2f center, out float radius);

                    circlesCenter.Add(center);

                    _contoursInfo.Add(new ContourInfo
                    {
                        ContourName = $"{counter}",
                        CentroidX = x,
                        CentroidY = y,
                        CircleX = center.X - _orig.Width / 2.0f,
                        CircleY = center.Y - _orig.Height / 2.0f,
                        CircleRadius = radius,
                        ContourArea = area,
                        ContourCircumference = circumference,
                        HistogramValues = tmpHistValues
                    });
                }
            }

            return circlesCenter;
        }
    }
}
