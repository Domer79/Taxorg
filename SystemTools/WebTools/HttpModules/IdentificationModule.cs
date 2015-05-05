 using System;
 using System.Web;
 using System.Web.Caching;
 using System.Web.Security;

namespace SystemTools.WebTools.HttpModules
{
    public class IdentificationModule : IHttpModule
    {
        /// <summary>
        /// �������������� ������ � �������������� ��� ��� ��������� ��������.
        /// </summary>
        /// <param name="context">������ <see cref="T:System.Web.HttpApplication"/>, ��������������� ������ � �������, ��������� � ��������, ���������� ������ ��� ���� �������� � ���������� ASP.NET.</param>
        public void Init(HttpApplication context)
        {
            if (!ApplicationCustomizer.EnableSecurity)
                return;
            context.PostAuthenticateRequest += PostAuthenticateRequest;
        }

        void PostAuthenticateRequest(object sender, System.EventArgs e)
        {
            var application = ((HttpApplication) sender);

            if (FormsAuthentication.IsEnabled)
            {
                ApplicationCustomizer.Security.Principal = ApplicationCustomizer.Security.GetWebPrinicipal();
            }
            else if (application.User.Identity.IsAuthenticated)
            {
                ApplicationCustomizer.Security.Principal = ApplicationCustomizer.Security.GetWindowsPrincipal(application.User.Identity.Name);
            }

            application.Context.User = ApplicationCustomizer.Security.Principal;
        }

        /// <summary>
        /// ������� ������� (����� ������), ������������ �������, ����������� <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
