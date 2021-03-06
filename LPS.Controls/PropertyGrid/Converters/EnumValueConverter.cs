﻿using System;
using System.Globalization;
using System.Windows.Data;
using LPS.Controls._PropertyGrid.Parts.Converters;

namespace LPS.Controls._PropertyGrid.Parts.Converters
{
	public class EnumValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return EnumHelper.GetValueWrapped(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((EnumWrapper)value).Value;
		}
	}
}
