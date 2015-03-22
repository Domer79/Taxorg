using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SystemTools.Extensions
{
    public static class StringExtensions
    {
        public static string SplitReverse(this IEnumerable<object> args, string delimiter)
        {
            if (args == null)
                return String.Empty;

            var enumerable = args as object[] ?? args.ToArray();
            var splitResult = !enumerable.Any() ? String.Empty : enumerable.Select(arg => arg.ToString()).Aggregate((str, next) => str + delimiter + next);
            return splitResult;
        }
        public static string SplitReverse(this IEnumerable<object> args)
        {
            return SplitReverse(args, ", ");
        }

        public static byte[] GetBytes(this string value)
        {
            return Encoding.Unicode.GetBytes(value);
        }

        public static bool RxIsMatch(this string value, string pattern)
        {
            return RxIsMatch(value, pattern, RegexOptions.None);
        }

        public static bool RxIsMatch(this string value, string pattern, RegexOptions options)
        {
            var rx = new Regex(pattern);
            return rx.IsMatch(value);
        }
    }
}
