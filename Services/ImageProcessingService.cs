using OpenCvSharp;
using SampleSegmenter.Models;
using SampleSegmenter.Options;
using System;
using System.Collections.Generic;

namespace SampleSegmenter.Services
{
    public class ImageProcessingService
    {
        private Mat _orig;
        private Mat _denoised;
        private Mat _grayscaled;
        private Mat _binarized;
        private Mat _result;

        private readonly List<ContourInfo> _contoursInfo;

        private bool _enableEqualized;

        private DenoiseOptions _denoiseOptions;
        private ThresholdOptions _thresholdOptions;
        private ContoursOptions _contoursOptions;

        public ImageProcessingService()
        {
            _denoiseOptions = new();
            _enableEqualized = true;
            _thresholdOptions = new();
            _contoursOptions = new();
            _contoursInfo = new();
        }

        public void SetOrigMat(Mat orig)
        {
            _orig = orig.Clone();
            Update();
        }

        public Mat GetDenoisedMat()
        {
            return _denoised.Clone();
        }

        public Mat GetGrayScaledMat()
        {
            return _grayscaled.Clone();
        }
        public Mat GetBinarizedMat()
        {
            return _binarized.Clone();
        }

        public Mat GetResultMat()
        {
            return _result.Clone();
        }

        public string GetContoursInfoText()
        {
            string header = "X\tY\tArea\tCircumference\n";
            string result = string.Join(Environment.NewLine, _contoursInfo);
            return header + result;
        }

        public List<ContourInfo> GetContoursInfo()
        {
            return _contoursInfo;
        }

        public void SetDenoiseOptions(DenoiseOptions denoiseOptions)
        {
            _denoiseOptions = denoiseOptions;
            Update();
        }

        public void SetEnableEqualized(bool enable)
        {
            _enableEqualized = enable;
            Update();
        }

        public void SetThresholdOptions(ThresholdOptions options)
        {
            _thresholdOptions = options;
            Update();
        }

        public void SetMinimumArea(ContoursOptions options)
        {
            _contoursOptions = options;
            Update();
        }

        private void Update()
        {
            Denoise();
            Grayscale();
            Threshold();
            Contours();
        }

        private void Denoise()
        {
            _denoised = _orig.Clone();
            Cv2.FastNlMeansDenoisingColored(_orig, _denoised, _denoiseOptions.H, _denoiseOptions.HColor, _denoiseOptions.TemplateWindowSize, _denoiseOptions.TemplateWindowSize);
        }

        private void Grayscale()
        {
            _grayscaled = _denoised.Clone();
            Cv2.CvtColor(_denoised, _grayscaled, ColorConversionCodes.BGR2GRAY);
            if (_enableEqualized) Cv2.EqualizeHist(_grayscaled, _grayscaled);
        }

        private void Threshold()
        {
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
            Point p1 = new(0, 0);
            Point p2 = new(_binarized.Cols, _binarized.Rows);
            int thickness = 2;
            Cv2.Rectangle(_binarized, p1, p2, new Scalar(255, 255, 255), thickness);

        }
        private void Contours()
        {
            _result = _orig.Clone();
            Cv2.FindContours(_binarized, out Point[][] contours, out HierarchyIndex[] hierarchyIndexes, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);

            var rand = new Random();
            var thickness = _contoursOptions.FillContours ? -1 : 2;

            _contoursInfo.Clear();

            foreach (HierarchyIndex hi in hierarchyIndexes)
            {
                if(hi.Next != -1 && hi.Parent == 0)
                {
                    var contour = contours[hi.Next];
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
        }
    }
}
