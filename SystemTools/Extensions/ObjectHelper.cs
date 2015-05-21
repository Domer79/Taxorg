using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SystemTools.Extensions
{
    public static class ObjectHelper
    {
        public static byte[] Serialize(this object @object)
        {
            if (@object == null) 
                throw new ArgumentNullException("object");

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, @object);
                return ms.GetBuffer();
            }
        }

        public static string SerializeToString(this object @object)
        {
            return @object.Serialize().GetString();
        }

        public static T DeserializeFromString<T>(string data)
        {
            return (T)DeserializeFromString(data);
        }

        public static object DeserializeFromString(string data)
        {
            return Deserialize(Encoding.Unicode.GetBytes(data));
        }

        public static T Deserialize<T>(byte[] data)
        {
            return (T)Deserialize(data);
        }

        public static object Deserialize(byte[] data)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                return bf.Deserialize(ms);
            }
        }
    }
}
