﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Globalization;

namespace LPS.Controls._PropertyGrid.Parts.Converters
{
    public class ColorConverter : TypeConverter
    {
        // Methods
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(SolidColorBrush)) ||(sourceType == typeof(Color)) || base.CanConvertFrom(context, sourceType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is SolidColorBrush) || !(value is Color))
            {
                return base.ConvertFrom(context, culture, value);
            }
            return new SolidColorBrush(Colors.Black);
        }
    }
}
