using System.Configuration;
using Action = System.Action;

namespace SystemTools.ConfigSections
{
    public class ErrorPage : ConfigurationSection
    {
        private const string UrlName = "url";
        private const string ControllerName = "controller";
        private const string ActionName = "action";

        [ConfigurationProperty(UrlName, DefaultValue = null)]
        public string Url
        {
            get { return (string)base[UrlName]; }
            set { base[UrlName] = value; }
        }

        [ConfigurationProperty(ControllerName, DefaultValue = null)]
        public string Controller
        {
            get { return (string) base[ControllerName] ?? "Error"; }
            set { base[ControllerName] = value; }
        }

        [ConfigurationProperty(ActionName, DefaultValue = null)]
        public string Action
        {
            get { return (string) base[ActionName] ?? "Index"; }
            set { base[ActionName] = value; }
        }
    }
}