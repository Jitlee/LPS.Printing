using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPS.Controls.PropertyGrid.Parts
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
            if (typeof(bool).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new BooleanValueEditor(item);
            }
            if (typeof(Enum).IsAssignableFrom(item.PropertyInfo.PropertyType))
            {
                return new EnumValueEditor(item);
            }
            return new StringValueEditor(item);
        }
    }
}
