using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using SystemTools.ConfigSections;
using SystemTools.Exceptions;
using SystemTools.Interfaces;

namespace SystemTools.WebTools.Infrastructure
{
    public class IdentificationModule : IHttpModule
    {
        /// <summary>
        /// Инициализирует модуль и подготавливает его для обработки запросов.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpApplication"/>, предоставляющий доступ к методам, свойствам и событиям, являющимся общими для всех объектов в приложении ASP.NET.</param>
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
        /// Удаляет ресурсы (кроме памяти), используемые модулем, реализующим <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
