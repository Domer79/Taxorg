using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            context.PostAuthenticateRequest += context_AuthenticateRequest;
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            var application = ((HttpApplication) sender);
            if (application.User.Identity.IsAuthenticated)
            {
                ApplicationCustomizer.OnAuthenticated(application.User.Identity);
                return;
            }

            if (application.Request.Cookies["username"] != null)
            {
                ApplicationCustomizer.OnAuthenticated(application.Request.Cookies["username"], application.Request.Cookies["password"]);
            }

            application.Response.ContentEncoding = Encoding.UTF8;
            application.Response.HeaderEncoding = Encoding.UTF8;
            application.Response.Write(@"<html><body>
                <h1>Идентификация</h1></body></html>");
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
