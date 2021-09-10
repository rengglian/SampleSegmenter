using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class MaskOptions : BindableBase
    {
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        private double _x = 230;
        public double X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        private double _y = 160;
        public double Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }

        private double _height = 250;
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        private double _width = 250;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }
    }
}
