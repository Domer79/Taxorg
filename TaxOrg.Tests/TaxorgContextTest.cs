using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlClr;
using TaxorgRepository;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;
using SystemTools;

namespace TaxOrg.Tests
{
    [TestClass]
    public class TaxorgContextTest
    {
        [TestMethod]
        public void TaxorgContextConnectionTest()
        {
            var isConnected = ConnectTry();

            Assert.IsTrue(isConnected);
        }

        protected bool ConnectTry()
        {
            return ConnectTry( //                        "Data Source=taxorg\vmssqlserver;Initial Catalog=Taxorg;User Id=developer;Password=sppdeveloper;Timeout=5")
                "Data Source=taxorg.garipov\vmssqlserver;Initial Catalog=Taxorg;User Id=developer;Password=sppdeveloper;Timeout=15");
//                        "Data Source=.;Initial Catalog=Taxorg;Integrated security=True;Timeout=5");
        }

        protected bool ConnectTry(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    Debug.WriteLine("Connection timeout: {0}", connection.ConnectionTimeout);
                    connection.Open();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Message: {0}, HelpLink: {1}", e.Message, e.HelpLink);
                    return false;
                }
            }
        }

        [TestMethod]
        public void SqlConnectionStringBuilderTest()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"taxorg\vmssqlserver";
            builder.InitialCatalog = "Taxorg";
            builder.UserID = "developer";
            builder.Password = "sppdeveloper";
            builder.NetworkLibrary = "dbmssocn";

            Assert.IsTrue(ConnectTry(builder.ConnectionString));
        }

        [TestMethod]
        public void TryConnect()
        {
            Assert.IsTrue(ConnectTry(ApplicationSettings.ConnectionString));
        }

        [TestMethod]
        public void ConnectionStringFromConfigTest()
        {
            Assert.AreEqual(ApplicationSettings.ConnectionString, "data source=.;initial catalog=Taxorg;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
        }

        [TestMethod]
        public void TaxSummaryGetTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;integrated security=True;";
            var repo = TaxSummaryRepository.Repository;

            foreach (var taxSummary in repo)
            {
                Debug.WriteLine(taxSummary.Inn);
            }
        }

        [TestMethod]
        public void SliceTaxGetTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;integrated security=True;";
            var repo = new SliceRepository(1989);

            foreach (var tax in repo)
            {
                Debug.WriteLine(tax.TaxDebitKredit);
            }
        }

        [TestMethod]
        public void GetCurrentPeriodTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;integrated security=True;";

            var period = TaxorgTools.GetCurrentPeriod();

            Assert.AreEqual(new YearMonth("02.2015"), period);
        }

        [TestMethod]
        public void GetPrevPeriodCountTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;integrated security=True;";
            var count = TaxorgTools.GetPrevPeriodCount();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void GetPrevPeriodTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;integrated security=True;";
            Assert.AreEqual(new YearMonth("02.2015"), TaxorgTools.GetPrevPeriod());
        }

        [TestMethod]
        public void IsNotSameTaxLoadTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;integrated security=True;";

            Assert.IsTrue(!TaxorgTools.IsNotSameTaxLoad);

            TaxorgTools.IsNotSameTaxLoad = true;

            Assert.IsTrue(TaxorgTools.IsNotSameTaxLoad);
        }

        [TestMethod]
        public void AppVersionTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;integrated security=True;";

            Assert.AreEqual("1.1.1.0", TaxorgTools.AppVersion);
        }

        [TestMethod]
        public void SetTaxPrevPeriodCountTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;integrated security=True;";

            TaxorgTools.SetTaxPrevPeriodCount(2);
        }
    }
}
