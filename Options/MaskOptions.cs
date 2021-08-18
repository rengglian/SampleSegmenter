using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class MaskOptions : BindableBase
    {
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }
    }
}
