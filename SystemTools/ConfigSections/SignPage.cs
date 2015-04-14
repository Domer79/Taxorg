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
            get { return (string)base[ControllerName] ?? "Logon"; }
            set { base[ControllerName] = value; }
        }

        [ConfigurationProperty(ActionName, IsRequired = false)]
        public string Action
        {
            get { return (string)base[ActionName] ?? "Index"; }
            set { base[ActionName] = value; }
        }
    }
}
