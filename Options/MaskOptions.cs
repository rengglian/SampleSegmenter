using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class MaskOptions : BindableBase
    {
        private bool _isEnabled = true;
        private double _x = 230;
        private double _y = 160;
        private double _height = 250;
        private double _width = 250;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }
    }
}
