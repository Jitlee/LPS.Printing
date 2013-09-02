﻿using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LPS.Controls.PropertyGrid.Parts
{
    public static class AttributeServices
    {
        public static T GetAttribute<T>(PropertyInfo propertyInfo)
        {
            object[] attributes = propertyInfo.GetCustomAttributes(typeof(T), true);
            return (attributes.Length > 0) ? attributes.OfType<T>().First() : default(T);
        }

        public static string GetCategory(PropertyInfo propertyInfo)
        {
            CategoryAttribute categroyAttribute = GetAttribute<CategoryAttribute>(propertyInfo);
            return string.IsNullOrEmpty(categroyAttribute.Category) ? "杂项" : categroyAttribute.Category;
        }

        public static string GetDisplayName(PropertyInfo propertyInfo)
        {
            DisplayNameAttribute displayNameAttribute = GetAttribute<DisplayNameAttribute>(propertyInfo);
            return string.IsNullOrEmpty(displayNameAttribute.DisplayName) ? propertyInfo.Name : displayNameAttribute.DisplayName;
        }

        public static string GetDescription(PropertyInfo propertyInfo)
        {
            DescriptionAttribute descriptionAttribute = GetAttribute<DescriptionAttribute>(propertyInfo);
            return descriptionAttribute.Description;
        }
    }
}