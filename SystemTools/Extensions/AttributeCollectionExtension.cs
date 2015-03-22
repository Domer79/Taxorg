using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SystemTools.Extensions
{
    /// <summary>
    /// Взято: http://www.codeproject.com/Articles/415070/Dynamic-Type-Description-Framework-for-PropertyGri
    /// </summary>
    public static class AttributeCollectionExtension
    {
        public static void Add(this System.ComponentModel.AttributeCollection ac, Attribute attribute)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            List<Attribute> listAttr = new List<Attribute>();
            if (arrAttr != null)
            {
                listAttr.AddRange(arrAttr);
            }
            listAttr.Add(attribute);
            fi.SetValue(ac, listAttr.ToArray());
        }

        public static void AddRange(this System.ComponentModel.AttributeCollection ac, Attribute[] attributes)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            List<Attribute> listAttr = new List<Attribute>();
            if (arrAttr != null)
            {
                listAttr.AddRange(arrAttr);
            }
            listAttr.AddRange(attributes);
            fi.SetValue(ac, listAttr.ToArray());
        }

        public static void Add(this System.ComponentModel.AttributeCollection ac, Attribute attribute, bool removeBeforeAdd)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            List<Attribute> listAttr = new List<Attribute>();
            if (arrAttr != null)
            {
                listAttr.AddRange(arrAttr);
            }
            if (removeBeforeAdd)
            {
                listAttr.RemoveAll(a => a.Match(attribute));
            }
            listAttr.Add(attribute);
            fi.SetValue(ac, listAttr.ToArray());
        }

        public static void Add(this System.ComponentModel.AttributeCollection ac, Attribute attribute, Type typeToRemoveBeforeAdd)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            List<Attribute> listAttr = new List<Attribute>();
            if (arrAttr != null)
            {
                listAttr.AddRange(arrAttr);
            }
            if (typeToRemoveBeforeAdd != null)
            {
                listAttr.RemoveAll(a => a.GetType() == typeToRemoveBeforeAdd || a.GetType().IsSubclassOf(typeToRemoveBeforeAdd));
            }
            listAttr.Add(attribute);
            fi.SetValue(ac, listAttr.ToArray());
        }

        public static void Clear(this System.ComponentModel.AttributeCollection ac)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(ac, null);
        }

        public static void Remove(this System.ComponentModel.AttributeCollection ac, Attribute attribute)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            List<Attribute> listAttr = new List<Attribute>();
            if (arrAttr != null)
            {
                listAttr.AddRange(arrAttr);
            }
            listAttr.RemoveAll(a => a.Match(attribute));
            fi.SetValue(ac, listAttr.ToArray());
        }

        public static void Remove(this System.ComponentModel.AttributeCollection ac, Type type)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            List<Attribute> listAttr = new List<Attribute>();
            if (arrAttr != null)
            {
                listAttr.AddRange(arrAttr);
            }
            listAttr.RemoveAll(a => a.GetType() == type);
            fi.SetValue(ac, listAttr.ToArray());
        }

        public static Attribute Get(this System.ComponentModel.AttributeCollection ac, Attribute attribute)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            if (arrAttr == null)
            {
                return null;
            }
            Attribute attrFound = arrAttr.FirstOrDefault(a => a.Match(attribute));
            return attrFound;
        }

        public static List<Attribute> Get(this System.ComponentModel.AttributeCollection ac, params Attribute[] attributes)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);

            if (arrAttr == null)
            {
                return null;
            }
            List<Attribute> listAttr = new List<Attribute>();
            listAttr.AddRange(arrAttr);
            System.ComponentModel.AttributeCollection ac2 = new System.ComponentModel.AttributeCollection(attributes);
            List<Attribute> listAttrFound = listAttr.FindAll(a => ac2.Matches(a));
            return listAttrFound;
        }

        public static Attribute Get(this System.ComponentModel.AttributeCollection ac, Type attributeType)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            Attribute attrFound = arrAttr.FirstOrDefault(a => a.GetType() == attributeType);
            return attrFound;
        }

        public static Attribute Get(this System.ComponentModel.AttributeCollection ac, Type attributeType, bool derivedType)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            Attribute attrFound = null;
            if (!derivedType)
            {
                attrFound = arrAttr.FirstOrDefault(a => a.GetType() == attributeType);
            }
            else
            {
                attrFound = arrAttr.FirstOrDefault(a => a.GetType() == attributeType || a.GetType().IsSubclassOf(attributeType));
            }
            return attrFound;
        }

        public static List<Attribute> Get(this System.ComponentModel.AttributeCollection ac, params Type[] attributeTypes)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);

            if (arrAttr == null)
            {
                return null;
            }
            List<Attribute> listAttr = new List<Attribute>();
            listAttr.AddRange(arrAttr);
            List<Attribute> listAttrFound = listAttr.FindAll(a => a.GetType() == attributeTypes.FirstOrDefault(b => b.GetType() == a.GetType()));

            return listAttrFound;
        }

        public static Attribute[] ToArray(this System.ComponentModel.AttributeCollection ac)
        {
            FieldInfo fi = ac.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
            Attribute[] arrAttr = (Attribute[])fi.GetValue(ac);
            return arrAttr;
        }

        /// <summary>
        /// Получения атрибута по типу (если несколько - первого)
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TAttribute GetAttributeValue<TAttribute>(this Type type)
        where TAttribute : Attribute
        {
            var attribute = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return attribute;
        }
    }
}
