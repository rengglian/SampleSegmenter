using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class DenoiseOptions : BindableBase
    {
        private float _h = 3f;
        public float H
        {
            get => _h;
            set => SetProperty(ref _h, value);
        }

        public float _hColor = 3f;
        public float HColor
        {
            get => _hColor;
            set => SetProperty(ref _hColor, value);
        }

        public int _templateWindowSize = 7;
        public int TemplateWindowSize
        {
            get => _templateWindowSize;
            set => SetProperty(ref _templateWindowSize, value);
        }
        public int _searchWindowSize = 21;
        public int SearchWindowSize
        {
            get => _searchWindowSize;
            set => SetProperty(ref _searchWindowSize, value);
        }
    }
}
