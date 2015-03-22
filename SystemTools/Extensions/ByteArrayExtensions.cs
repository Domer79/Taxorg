using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
