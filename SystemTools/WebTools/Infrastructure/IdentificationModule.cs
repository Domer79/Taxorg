using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
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
            context.EndRequest += context_EndRequest;
        }

        void context_EndRequest(object sender, System.EventArgs e)
        {
//            throw new NotImplementedException();
        }

        void context_AuthenticateRequest(object sender, System.EventArgs e)
        {
            var application = ((HttpApplication) sender);

            var signPage = AdditionalConfiguration.Instance.SignPage;

            if (application.User.Identity.IsAuthenticated)
            {
                ApplicationCustomizer.OnAuthenticated(application.User.Identity);
                return;
            }

            if (application.Request[ApplicationCustomizer.SecurityCookieName] != null)
            {
                ApplicationCustomizer.OnAuthenticated((ISecurity)application.Session[ApplicationCustomizer.SecurityCookieName]);
                return;
            }

//            application.Response.RedirectToRoute(new RouteValueDictionary(new {controller = "Logon", action = "Index"}));

            if (signPage == null)
                throw new LogonException();

            var url = new UriBuilder(application.Request.Url.GetLeftPart(UriPartial.Authority));
            url.Path = HttpRuntime.AppDomainAppVirtualPath;
            
            application.Response.Redirect(url.Uri.PathAndQuery);
            application.Response.End();
        }

        /// <summary>
        /// Удаляет ресурсы (кроме памяти), используемые модулем, реализующим <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
