using OpenCvSharp;
using Prism.Mvvm;
using SampleSegmenter.Converters;
using SampleSegmenter.Enums;
using SampleSegmenter.Interfaces;
using SampleSegmenter.Models;
using SampleSegmenter.Options;
using System;
using System.Collections.Generic;
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
        private ThresholdOptions _thresholdOptions;
        private MaskOptions _maskOptions;
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
            _thresholdOptions = new();
            _maskOptions = new();
            _dilateOptions = new();
            _contoursOptions = new();
            _contoursInfo = new();
        }

        public void SetOrigMat(Mat orig)
        {
            Information = "Set Original Image";
            _orig = orig.Clone();
            _maskOptions = new();
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
            if (optionType == typeof(ThresholdOptions)) { _thresholdOptions = options as ThresholdOptions; }
            if (optionType == typeof(MaskOptions)) { _maskOptions = options as MaskOptions; }
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
                case ImageProcessingSteps.Binarized:
                    {
                        Image = ImageConverter.Convert(_binarized.Clone());
                        break;
                    }
                case ImageProcessingSteps.Masked:
                    {
                        Image = ImageConverter.Convert(_masked.Clone());
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
            Denoise();
            Grayscale();
            Threshold();
            Mask();
            Dilate();
            Contours();
            UpdateImage(SelectedImageProcessingStep);
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

        private void Threshold()
        {
            Information = "Binarize Image";

            ThresholdTypes thresholdTypes;
            if (_thresholdOptions.UseOtsu)
            {
                thresholdTypes = ThresholdTypes.Otsu | ThresholdTypes.BinaryInv;
            }
            else
            {
                thresholdTypes = ThresholdTypes.BinaryInv;
            }
            

            _binarized = _grayscaled.Clone();
            Cv2.Threshold(_grayscaled, _binarized, _thresholdOptions.ThresholdValue, _thresholdOptions.MaxValue, thresholdTypes);
        }

        private void Mask()
        {
            Information = "Mask Image";
            if (_maskOptions.IsEnabled)
            {
                var rect = new Rect((int)_maskOptions.X, (int)_maskOptions.Y, (int)_maskOptions.Width, (int)_maskOptions.Height);
                var roi = new Mat(_binarized, rect);
                _masked = roi.Clone();
            }
            else
            {
                _masked = _binarized.Clone();
            }

        }

        private void Dilate()
        {
            Information = "Dilate Image";
            _dilated = _masked.Clone();

            if (_dilateOptions.IsEnabled)
            {
                var struct_element = Cv2.GetStructuringElement(
                    MorphShapes.Cross,
                    new Size(2 * _dilateOptions.Size + 1, 2 * _dilateOptions.Size + 1),
                    new Point(_dilateOptions.Size, _dilateOptions.Size));

                Cv2.Dilate(_masked, _dilated, struct_element, iterations: _dilateOptions.Iterations);
            }
        }

        private void Contours()
        {
            Information = "Find Contours";
            _result = _orig.Clone();
            Cv2.FindContours(_masked, out Point[][] contours, out HierarchyIndex[] hierarchyIndexes, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);

            var rand = new Random();
            var thickness = _contoursOptions.FillContours ? -1 : 2;

            _contoursInfo.Clear();

            foreach (HierarchyIndex hi in hierarchyIndexes)
            {
                Information = @"Analyse Contour " + hi.Next;
                if(hi.Next != -1 && hi.Parent == 0)
                {
                    var contour = contours[hi.Next];
                    if(_contoursOptions.ConvexHull)
                    {
                        contour = Cv2.ConvexHull(contour, true);
                        contours[hi.Next] = contour;
                    }
                    var area = Cv2.ContourArea(contour);
                    if(area > _contoursOptions.MinimumArea)
                    {
                        Moments moment = Cv2.Moments(contour);
                        var x = (int)(moment.M10 / moment.M00);
                        var y = (int)(moment.M01 / moment.M00);

                        double circumference = Cv2.ArcLength(contour, true);
                        var randomColor = new Scalar(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), 255);
                        Cv2.DrawContours(
                            _result,
                            contours,
                            hi.Next,
                            color: randomColor,
                            thickness: thickness,
                            lineType: LineTypes.Link8,
                            hierarchy: hierarchyIndexes,
                            maxLevel: int.MaxValue);

                        _contoursInfo.Add(new ContourInfo
                        {
                            X = x,
                            Y = y,
                            Area = area,
                            Circumference = circumference
                        });
                    }
                }
            }
            IsImageLoaded = true;
            Information = "Done";
        }
    }
}
