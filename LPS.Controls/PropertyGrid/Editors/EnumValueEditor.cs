using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class EnumValueEditor : ComboBoxValueEditor
    {
        public EnumValueEditor(PropertyItem item)
            : base(item)
        {
        }

        protected override string GetDisplayMemberPath()
        {
            return "Value";
        }

        protected override string GetSelectValuePath()
        {
            return "Key";
        }

        protected override IEnumerable GetItemSource()
        {
            if (null == base.Item.Property.Source)
            {
                Type type = base.Item.PropertyInfo.PropertyType;
                string[] names = Enum.GetNames(type);

                Dictionary<object, object> items = new Dictionary<object, object>();
                foreach (string name in names)
                {
                    DescriptionAttribute decriptionAttribute = AttributeServices.GetAttribute<DescriptionAttribute>(type.GetField(name));
                    items.Add(Enum.Parse(type, name, true), null != decriptionAttribute ? decriptionAttribute.Description : name);
                }
                return items;
            }
            return base.Item.Property.Source;
        }
    }
}
