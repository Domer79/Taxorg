using System;
using System.Diagnostics;

namespace SystemTools.Extensions
{
    public static class ExceptionExtensions
    {
        public static void SaveError(this Exception e)
        {
            if (e == null) 
                throw new ArgumentNullException("e");

            if (ApplicationCustomizer.SaveErrorLog != null)
                try
                {
                    ApplicationCustomizer.SaveErrorLog(e);
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc);
                }
            else
            {
                Debug.WriteLine(e);
            }
        }

        public static string GetErrorMessage(this Exception e)
        {
            if (e == null)
                return string.Empty;

            return string.Format("{0}. Внутреннее сообщение: {1}", e.Message, GetErrorMessage(e.InnerException));
        }
    }
}
