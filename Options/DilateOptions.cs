using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class DilateOptions : BindableBase
    {
        private bool _isEnabled = true;
        private int _size = 1;
        private int _iterations = 1;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public int Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public int Iterations
        {
            get => _iterations;
            set => SetProperty(ref _iterations, value);
        }
    }
}
