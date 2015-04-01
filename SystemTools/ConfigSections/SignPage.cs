using System.Configuration;

namespace SystemTools.ConfigSections
{
    public class SignPage : ConfigurationSection
    {
        [ConfigurationProperty("logonurl", IsRequired = true)]
        public string LogonUrl
        {
            get { return (string)base["logonurl"]; }
            set { base["logonurl"] = value; }
        }
    }
}
