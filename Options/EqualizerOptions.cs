using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class EqualizerOptions : BindableBase
    {
        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
    }
}
