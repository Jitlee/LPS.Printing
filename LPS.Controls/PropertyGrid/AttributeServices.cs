using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LPS.Controls._PropertyGrid.Parts
{
    public static class AttributeServices
    {
        public static T GetAttribute<T>(MemberInfo memberInfo)
        {
            object[] attributes = memberInfo.GetCustomAttributes(typeof(T), true);
            return (attributes.Length > 0) ? attributes.OfType<T>().First() : default(T);
        }

        public static string GetCategory(MemberInfo memberInfo)
        {
            CategoryAttribute categroyAttribute = GetAttribute<CategoryAttribute>(memberInfo);
            return null == categroyAttribute ? "杂项" : categroyAttribute.Category;
        }

        public static string GetDisplayName(MemberInfo memberInfo)
        {
            DisplayNameAttribute displayNameAttribute = GetAttribute<DisplayNameAttribute>(memberInfo);
            return null == displayNameAttribute ? memberInfo.Name : displayNameAttribute.DisplayName;
        }

        public static string GetDescription(MemberInfo memberInfo)
        {
            DescriptionAttribute descriptionAttribute = GetAttribute<DescriptionAttribute>(memberInfo);
            return null == descriptionAttribute ? string.Empty : descriptionAttribute.Description;
        }
    }
}
