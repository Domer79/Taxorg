using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools.ConfigSections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemTools.Tests
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void AdditionalConfigurationTest()
        {
            var conf = AdditionalConfiguration.GetAdditionalConfiguration(@"c:\Projects\Taxorg\TaxOrg\web.config");
            Debug.WriteLine(conf.SignPage.LogonUrl);
        }
    }
}
