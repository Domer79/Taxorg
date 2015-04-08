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
            Debug.WriteLine(conf.SignPage.Controller, conf.SignPage.Action);
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

        [TestMethod]
        public void PathCollectionGetPathTest()
        {
            var pathCollection = new PathCollection("/qwe/rty/uio/asd/fgh/jkl/zxc/vbn/");
            Debug.WriteLine(pathCollection.Path);
        }

        [TestMethod]
        public void PathCollectionSetPathTest()
        {
            var pathCollection = new PathCollection("/qwe/rty/uio/asd/fgh/jkl/zxc/vbn/");
            pathCollection.Path += "path1/path2/path3/";
            Debug.WriteLine(pathCollection.Path);
        }

        [TestMethod]
        public void UrlBuilderQueryPatternTest()
        {
            const string pattern = @"^?(?<name>[\w]+)=(?<value>[\w]*)";
            const string query = "?name1=value1&name2=value2&name3=&name4=value4";

            var matchCollection = Regex.Matches(query, pattern);

            foreach (var match in matchCollection.OfType<Match>())
            {
                Debug.WriteLine("{0} = {1}", match.Groups["name"].Value, match.Groups["value"].Value);
            }
        }

        [TestMethod]
        public void QueryCollectionCreateTest()
        {
            const string query = "?name1=value1&name2=value2&name3=&name4=value4";
            var queryCollection = new QueryCollection(query);

            foreach (var keyValue in queryCollection)
            {
                Debug.WriteLine("{0} = {1}", keyValue.Key, keyValue.Value);
            }

            Assert.AreEqual(queryCollection.Query, query);
        }

        [TestMethod]
        public void QueryCollectionSetQueryTest()
        {
            const string query = "?name1=value1&name2=value2&name3=&name4=value4";
            var queryCollection = new QueryCollection(query);

            Debug.WriteLine(queryCollection.Query);
        }

        [TestMethod]
        public void ObjectEqualsTest()
        {
            var s1 = new string("Hello World!!!".ToCharArray());
            var s2 = new string("Hello World!!!".ToCharArray());

            Assert.IsTrue(s1.Equals(s2));
        }

        #region UriBuilder2 Test

        [TestMethod]
        public void UriBuilderCreateTest()
        {
            var uriBuilder = new UriBuilder2("http://localhost/Taxorg?name1=value1");
            uriBuilder.Path += "org/index";
            uriBuilder.UserName = "Domer";
            uriBuilder.Password = "password";

            Debug.WriteLine(uriBuilder.Uri.AbsolutePath);
            Debug.WriteLine(uriBuilder.Uri.AbsoluteUri);
            Debug.WriteLine(uriBuilder.Uri.Query);
        }

        #endregion
    }
}
