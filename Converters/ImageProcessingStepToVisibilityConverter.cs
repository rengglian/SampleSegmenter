﻿using SampleSegmenter.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SampleSegmenter.Converters
{
    public class ImageProcessingStepToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((ImageProcessingSteps)value == ImageProcessingSteps.Orignal)
			{
				return Visibility.Visible;
			}
			return  Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> DependencyProperty.UnsetValue;
	}
}
