using System.Web;
using System.Web.Security;

namespace SystemTools.WebTools.HttpModules
{
    public class IdentificationModule : IHttpModule
    {
        /// <summary>
        /// »нициализирует модуль и подготавливает его дл€ обработки запросов.
        /// </summary>
        /// <param name="context">ќбъект <see cref="T:System.Web.HttpApplication"/>, предоставл€ющий доступ к методам, свойствам и событи€м, €вл€ющимс€ общими дл€ всех объектов в приложении ASP.NET.</param>
        public void Init(HttpApplication context)
        {
            if (!ApplicationCustomizer.EnableSecurity)
                return;
            context.PostAuthenticateRequest += context_AuthenticateRequest;
        }

        void context_AuthenticateRequest(object sender, System.EventArgs e)
        {
            var application = ((HttpApplication) sender);

            if (application.User.Identity.IsAuthenticated)
            {
//                application.Context.User = ApplicationCustomizer.Security.GetWindowsPrincipal(application.User.Identity.Name);
                return;
            }

            if (application.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                application.Context.User = ApplicationCustomizer.Security.GetWebPrinicipal();
            }
        }

        /// <summary>
        /// ”дал€ет ресурсы (кроме пам€ти), используемые модулем, реализующим <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
