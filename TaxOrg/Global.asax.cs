using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SystemTools;
using SystemTools.ConfigSections;
using SystemTools.WebTools.Infrastructure;
using Antlr.Runtime.Misc;
using TaxorgRepository;
using TaxorgRepository.Repositories;
using WebSecurity;

namespace TaxOrg
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ControllerBuilder.Current.SetControllerFactory(new SecurityControllerFactory());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ApplicationCustomizer.RegisterErrorLog(ErrorLog.SaveError);
            ApplicationCustomizer.AppVersion = TaxorgTools.AppVersion;
//            ApplicationCustomizer.Security = Security.Instance;
//            ApplicationCustomizer.EnableSecurity = true;
//            ApplicationCustomizer.SecurityConnectionString = AdditionalConfiguration.Instance.SecurityConnectionString;
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Server.ClearError();
            Session["errorObject"] = exception;
            ApplicationCustomizer.IsError = true;
            Response.RedirectToRoute(new {Controller = "Org", Action = "Error"});
        }
    }
}
