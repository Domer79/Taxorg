using System.Configuration;

namespace SystemTools.ConfigSections
{
    public class SignPage : ConfigurationSection
    {
        private const string ControllerName = "controller";
        private const string ActionName = "action";

        [ConfigurationProperty(ControllerName, IsRequired = true)]
        public string Controller
        {
            get { return (string)base[ControllerName]; }
            set { base[ControllerName] = value; }
        }

        [ConfigurationProperty(ActionName, IsRequired = false)]
        public string Action
        {
            get { return base[ActionName] == null ? "Index" : (string)base[ActionName]; }
            set { base[ActionName] = value; }
        }
    }
}
