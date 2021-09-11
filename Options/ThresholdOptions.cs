using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class ThresholdOptions : BindableBase
    {
        private bool _useOtsu = true;
        public bool UseOtsu
        {
            get => _useOtsu;
            set => SetProperty(ref _useOtsu, value);
        }

        private bool _invert = true;
        public bool Invert
        {
            get => _invert;
            set => SetProperty(ref _invert, value);
        }

        private double _thresholdValue = 0.0;
        public double ThresholdValue
        {
            get => _thresholdValue;
            set => SetProperty(ref _thresholdValue, value);
        }
        private double _maxValue = 255.0;
        public double MaxValue
        {
            get => _maxValue;
            set => SetProperty(ref _maxValue, value);
        }
    }
}
