using System;
using System.Globalization;
using System.Windows.Data;
using LPS.Controls.PropertyGrid.Parts.Converters;

namespace LPS.Controls.PropertyGrid.Parts.Converters
{
	public class EnumTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return EnumHelper.GetValues(value.GetType());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

	}
}
