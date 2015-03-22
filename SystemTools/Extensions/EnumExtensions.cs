using System;
using System.ComponentModel;

namespace SystemTools.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attr = ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo,
                    typeof (DescriptionAttribute)));

            return attr == null ? enumValue.ToString() : attr.Description;
        }
    }
}
