using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LPS.Controls._PropertyGrid.Parts
{
    public static class ValueEditorServices
    {
        public static ValueEditorBase CreateValueEdiorBase(PropertyItem item)
        {
            ImageAttribute imageAttribute = AttributeServices.GetAttribute<ImageAttribute>(item.PropertyInfo);
            if (null != imageAttribute)
            {
                return new ImageValueEditor(item, imageAttribute);
            }
            if (typeof(Boolean).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new BooleanValueEditor(item);
            }
            if (typeof(Color).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new ColorValueEditor(item);
            }
            if (typeof(Brush).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new ColorValueEditor(item);
            }
            if (typeof(Thickness).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new ThicknessValueEditor(item);
            }
            if (typeof(CornerRadius).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new CornerRadiusValueEditor(item);
            }
            if (typeof(FontFamily).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new FontFamilyValueEditor(item);
            }
            if (typeof(FontStyle).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new FontStyleValueEditor(item);
            }
            if (typeof(FontWeight).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new FontWeightValueEditor(item);
            }


            // ------------------------------------------------------------------------
            if (typeof(Enum).IsAssignableFrom(item.PropertyInfo.PropertyType) ||
                null != item.Property.Source)
            {
                return new EnumValueEditor(item);
            }
            return new StringValueEditor(item);
        }
    }
}
