using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace LPS.Controls.PropertyGrid.Parts.Converters
{
    public class FontFamilyConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
		}

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    return new FontFamily((string)value);
                }
                catch (FormatException exception)
                {
                    throw new FormatException(string.Format("字体转换失败 {0} - {1}", (string)value, "FontFamily"), exception);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
	}
}
