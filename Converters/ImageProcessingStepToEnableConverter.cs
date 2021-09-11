using SampleSegmenter.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SampleSegmenter.Converters
{
    public class ImageProcessingStepToEnableConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((ImageProcessingSteps)value == ImageProcessingSteps.Orignal)
			{
				return true;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> DependencyProperty.UnsetValue;
	}
}
