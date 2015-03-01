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
                ApplicationCustomizer.SaveErrorLog(e);
            else
            {
                Debug.WriteLine(e);
            }
        }
    }
}
