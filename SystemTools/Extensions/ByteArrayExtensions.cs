using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SystemTools.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] bytes)
        {
            var charArray = Encoding.Unicode.GetChars(bytes);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(charArray);
            return stringBuilder.ToString();
        }

        public static byte[] GetHashBytes(this string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(value.GetBytes());
        }
    }
}
