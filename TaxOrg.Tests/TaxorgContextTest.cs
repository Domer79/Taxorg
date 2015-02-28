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
using WebTools;

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
    }
}
