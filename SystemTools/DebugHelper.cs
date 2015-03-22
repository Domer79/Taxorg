using System;

namespace SystemTools
{
    public class DebugHelper
    {
        public static void ThrowErrorIfDebugMode(Exception exception)
        {
            if (IfDebugMode)
                throw exception;
        }

        public static void WriteLine(object line)
        {
            if (IfDebugMode)
                Console.WriteLine(line);
        }

        public static void Write(object line)
        {
            if (IfDebugMode)
                Console.Write(line);
        }

        public static bool IfDebugMode
        {
            get
            {
                bool res = false;
#if DEBUG
                res = true;
#endif
                return res;
            }
        }
    }
}
