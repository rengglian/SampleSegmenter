﻿using Prism.Mvvm;

namespace SampleSegmenter.Options
{
    public class ContoursOptions : BindableBase
    {
        private bool _fillContours = true;
        public bool FillContours
        {
            get { return _fillContours; }
            set { SetProperty(ref _fillContours, value); }
        }

        private bool _convexHull;
        public bool ConvexHull
        {
            get { return _convexHull; }
            set { SetProperty(ref _convexHull, value); }
        }

        private bool _advanced;
        public bool Advanced
        {
            get { return _advanced; }
            set { SetProperty(ref _advanced, value); }
        }

        private double _minimumArea = 10.0;
        public double MinimumArea
        {
            get { return _minimumArea; }
            set { SetProperty(ref _minimumArea, value); }
        }
    }
}
