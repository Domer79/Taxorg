using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SystemTools.ConfigSections;
using SystemTools.WebTools.Infrastructure;
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

        [TestMethod]
        public void UriSlashTest()
        {
            var uri = "jhgjhgjh";
            foreach (var s in uri.Split('/'))
            {
                Debug.WriteLine(string.Format("path: {0}", s));                
            }
        }

        [TestMethod]
        public void UriTrimSlashTest()
        {
            var uri = "/jgh/ert/cxvb/";
            var trimUri = new Regex("[^/].+[^/]").Match(uri).Value;
            Debug.WriteLine(string.Format("triUri:{0}", trimUri));
        }

        [TestMethod]
        public void PathCollectionTest()
        {
            var pathCollection = new PathCollection("wer/rty/sdf");
            foreach (var path in pathCollection)
            {
                Debug.WriteLine(path);
            }
        }

        [TestMethod]
        public void PathCollectionContainsTest()
        {
            var pathCollection = new PathCollection("/qwe/rty/uio/asd/fgh/jkl/zxc/vbn/");
            Assert.IsTrue(pathCollection.Contains("rty/uio/asd"));
        }
    }
}
