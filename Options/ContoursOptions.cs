using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class ContoursOptions : BindableBase
    {
        private bool _fillContours = true;
        public bool FillContours
        {
            get => _fillContours;
            set => SetProperty(ref _fillContours, value);
        }

        private bool _convexHull;
        public bool ConvexHull
        {
            get => _convexHull;
            set => SetProperty(ref _convexHull, value);
        }

        private bool _advanced;
        public bool Advanced
        {
            get => _advanced;
            set => SetProperty(ref _advanced, value);
        }

        private double _minimumArea = 10.0;
        public double MinimumArea
        {
            get => _minimumArea;
            set => SetProperty(ref _minimumArea, value);
        }
    }
}
