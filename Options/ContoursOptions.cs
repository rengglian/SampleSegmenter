using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class ContoursOptions : BindableBase
    {
        private bool _fillContours = true;
        public bool FillContours
        {
            get { return _fillContours; }
            set { SetProperty(ref _fillContours, value); }
        }

        private double _minimumArea = 1000.0;
        public double MinimumArea
        {
            get { return _minimumArea; }
            set { SetProperty(ref _minimumArea, value); }
        }
    }
}
