using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SystemTools;
using SystemTools.WebTools.Infrastructure;
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
            ApplicationCustomizer.Security = Security.Instance;
            ApplicationCustomizer.EnableSecurity = true;
        }

        public override void Init()
        {
            base.Init();
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            return base.GetVaryByCustomString(context, custom);
        }

        public override string GetOutputCacheProviderName(HttpContext context)
        {
            return base.GetOutputCacheProviderName(context);
        }
    }
}
