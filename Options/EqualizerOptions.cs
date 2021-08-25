using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class EqualizerOptions : BindableBase
    {
        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }
    }
}
