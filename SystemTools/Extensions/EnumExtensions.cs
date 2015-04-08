using System;
using System.ComponentModel;
using System.Linq;

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

        public static Enum[] GetFlags(this Enum @enum)
        {
            var values = Enum.GetValues(@enum.GetType());
            return values.OfType<Enum>().Where(@enum.HasFlag).ToArray();
        }
    }
}
