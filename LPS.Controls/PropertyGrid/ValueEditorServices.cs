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
            if (typeof(Enum).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new EnumValueEditor(item);
            }
            if (typeof(Color).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new ColorValueEditor(item);
            }
            if (typeof(SolidColorBrush).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new ColorValueEditor(item);
            }
            if (typeof(Thickness).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new ThicknessValueEditor(item);
            }
            return new StringValueEditor(item);
        }
    }
}
