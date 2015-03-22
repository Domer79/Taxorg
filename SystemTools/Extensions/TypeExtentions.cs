using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemTools.Extensions
{
    public static class TypeExtentions
    {
        public static IEnumerable<Type> GetParentTypes(this Type type)
        {
            while (type != null)
            {
                var t = type;
                type = type.BaseType;
                yield return t;
            }
        }

        public static bool Is(this Type type, Type parentType)
        {
            return GetParentTypes(type).Any(t => t == parentType);
        }

        public static bool Is(this object @object, Type parentType)
        {
            if (@object == null) 
                throw new ArgumentNullException("object");

            return @object.GetType().Is(parentType);
        }

        public static bool Is<T>(this Type type)
        {
            return Is(type, typeof (T));
        }

        public static bool Is<T>(this object @object)
        {
            if (@object == null) 
                throw new ArgumentNullException("object");

            return @object.Is(typeof (T));
        }

        public static T As<T>(this object @object) 
            where T : class
        {
            if (@object.Is<T>())
                return (T) @object;

            return null;
        }
    }
}
