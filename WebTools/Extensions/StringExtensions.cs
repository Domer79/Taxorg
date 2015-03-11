using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
