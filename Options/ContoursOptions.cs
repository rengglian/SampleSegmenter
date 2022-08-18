using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class ContoursOptions : BindableBase
    {
        private bool _fillContours = true;
        private bool _convexHull;
        private bool _advanced;
        private double _minimumArea = 10.0;

        public bool FillContours
        {
            get => _fillContours;
            set => SetProperty(ref _fillContours, value);
        }
        
        public bool ConvexHull
        {
            get => _convexHull;
            set => SetProperty(ref _convexHull, value);
        }

        public bool Advanced
        {
            get => _advanced;
            set => SetProperty(ref _advanced, value);
        }

        public double MinimumArea
        {
            get => _minimumArea;
            set => SetProperty(ref _minimumArea, value);
        }
    }
}
