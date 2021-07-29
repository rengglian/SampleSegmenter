using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class DilateOptions : BindableBase
    {
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        private int _size = 1;
        public int Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }

        private int _iterations = 1;
        public int Iterations
        {
            get { return _iterations; }
            set { SetProperty(ref _iterations, value); }
        }
    }
}
