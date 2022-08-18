﻿using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class CannyEdgeOptions : BindableBase
    {
        private int _scalePercentage = 10;
        public int ScalePercentage
        {
            get => _scalePercentage;
            set => SetProperty(ref _scalePercentage, value);
        }
    }
}