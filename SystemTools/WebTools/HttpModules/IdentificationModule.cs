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
                application.Context.User = ApplicationCustomizer.Security.GetWebPrinicipal();
                return;
            }

            if (application.User.Identity.IsAuthenticated)
            {
                application.Context.User = ApplicationCustomizer.Security.GetWindowsPrincipal(application.User.Identity.Name);
                return;
            }
        }

        /// <summary>
        /// ������� ������� (����� ������), ������������ �������, ����������� <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
