﻿using System;
using System.Globalization;

namespace LPS.Controls._PropertyGrid.Parts.Converters
{
	public class UInt16Converter : BaseNumberConverter
	{
		// Methods
		internal override object FromString(string value, CultureInfo culture)
		{
			return ushort.Parse(value, culture);
		}

		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return ushort.Parse(value, NumberStyles.Integer, (IFormatProvider)formatInfo);
		}

		internal override object FromString(string value, int radix)
		{
			return Convert.ToUInt16(value, radix);
		}

		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			ushort num = (ushort)value;
			return num.ToString("G", formatInfo);
		}

		// Properties
		internal override Type TargetType
		{
			get
			{
				return typeof(ushort);
			}
		}
	}
}
