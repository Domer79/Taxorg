using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SystemTools;
using TaxorgRepository.Repositories;

namespace TaxOrg
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ApplicationCustomizer.RegisterErrorLog(ErrorLog.SaveError);
        }
    }
}
