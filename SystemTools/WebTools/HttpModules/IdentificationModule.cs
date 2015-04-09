using System.Web;
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
        /// ������� ������� (����� ������), ������������ �������, ����������� <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
