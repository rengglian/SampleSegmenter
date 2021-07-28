using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class ThresholdOptions : BindableBase
    {
        private bool _useOtsu = true;
        public bool UseOtsu
        {
            get { return _useOtsu; }
            set { SetProperty(ref _useOtsu, value); }
        }

        private double _thresholdValue = 0.0;
        public double ThresholdValue
        {
            get { return _thresholdValue; }
            set { SetProperty(ref _thresholdValue, value); }
        }
        private double _maxValue = 255.0;
        public double MaxValue
        {
            get { return _maxValue; }
            set { SetProperty(ref _maxValue, value); }
        }
    }
}
