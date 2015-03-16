using System;
using System.Web;
using System.Security.Cryptography;

namespace TaxOrg.Controllers
{
    public class CheckSession : IHttpModule
    {
        public CheckSession()
        {
        }

        public String ModuleName
        {
            get { return "CheckSession"; }
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest += (new EventHandler(this.Application_BeginRequest));
            application.EndRequest += (new EventHandler(this.Application_EndRequest));
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication) source;
            HttpContext context = application.Context;
            string filePath = context.Request.FilePath;
            string fileExtension = VirtualPathUtility.GetExtension(filePath);
            if (fileExtension.Equals(".aspx") || fileExtension.Equals(".ashx"))
            {
                if (context.Request.Cookies["id"] != null)
                {
                    if (context.Request.Cookies["id"].Value.ToString().Length >= 32)
                    {
                        string cook = context.Request.Cookies["id"].Value.Substring(0, 32);
                        string ip_v = context.Request.ServerVariables["REMOTE_ADDR"].Trim();
                        string br_v = context.Request.ServerVariables["HTTP_USER_AGENT"].Trim();
                        if ((ip_v != "") && (br_v != ""))
                        {
                            if (crypt.getmd5(ip_v + br_v) != cook)
                            {
                                if (context.Request.ServerVariables["URL"] != "/errors/cookie.html")
                                    context.Response.Redirect("/errors/cookie.html");
                            }
                        }
                    }
                }
            }
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
        }

        public void Dispose()
        {
        }
    }
}