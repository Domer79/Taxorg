using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using SystemTools.WebTools.Infrastructure;
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
        const string SessionId = "qwertyuiop";

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public TaxorgContextTest()
        {
            ApplicationCustomizer.ConnectionString = "data source=.;initial catalog=Taxorg;User Id=developer;Password=sppdeveloper;MultipleActiveResultSets=True;App=EntityFramework";
        }

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
        public void SliceTaxGetTest()
        {
            var repo = new SliceRepository(1989);

            foreach (var tax in repo)
            {
                Debug.WriteLine(tax.TaxDebitKredit);
            }
        }

        [TestMethod]
        public void GetCurrentPeriodTest()
        {
            var period = TaxorgTools.GetCurrentPeriod();

            Assert.AreEqual(new YearMonth("02.2015"), period);
        }

        [TestMethod]
        public void GetPrevPeriodCountTest()
        {
            var count = TaxorgTools.GetPrevPeriodCount();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void GetPrevPeriodTest()
        {
            Assert.AreEqual(new YearMonth("02.2015"), TaxorgTools.GetPrevPeriod());
        }

        [TestMethod]
        public void IsNotSameTaxLoadTest()
        {
            Assert.IsTrue(!TaxorgTools.IsNotSameTaxLoad);

            TaxorgTools.IsNotSameTaxLoad = true;

            Assert.IsTrue(TaxorgTools.IsNotSameTaxLoad);
        }

        [TestMethod]
        public void AppVersionTest()
        {
            Assert.AreEqual("1.1.1.0", TaxorgTools.AppVersion);
        }

        [TestMethod]
        public void SetTaxPrevPeriodCountTest()
        {
            TaxorgTools.SetTaxPrevPeriodCount(2);
        }

        [TestMethod]
        public void TaxOnlySummaryTest()
        {
            const bool sessionIsAdd = true;
            var context = new TaxorgContext();
            SetSessionTestValues(context, sessionIsAdd);

            var query = context.Taxes.AsQueryable();
            var taxSource = context.SessionTaxTypes.Where(stt => stt.SessionId == SessionId).SelectMany(e => e.TaxType.Taxes);
            if (taxSource.Any())
                query = query.Intersect(taxSource);
//            var joinQuery = query.Join(context.SessionTaxTypes, t => t.IdTaxType, stt => stt.IdTaxType, (tax, type) => new {tax.TaxSum, tax.Organization, tax.PeriodName, tax.IdTaxType});
            var groupedQuery = query
                .GroupBy(e => new {e.Organization, e.PeriodName})
                .Select(e => new {Tax = e.Sum(t => t.TaxSum), e.Key.Organization, e.Key.PeriodName, PrevTax = context.Taxes.Intersect(taxSource).Where(tax => tax.PeriodName == "01.2015" && tax.Organization == e.Key.Organization).Select(tax => tax == null ? 0 : tax.TaxSum).Sum()})
                .Where(e => e.PeriodName == "02.2015");

            groupedQuery = groupedQuery.Take(10);
            var list = groupedQuery.ToList();

            foreach (var tax in list)
//            foreach (var tax in query)
            {
                Debug.WriteLine("{0};{1};{2}", tax.Tax, tax.Organization.Inn, tax.PrevTax);
            }
        }

        private static void SetSessionTestValues(TaxorgContext context, bool sessionIsAdd)
        {
            var session = context.Sessions.FirstOrDefault(s => s.SessionId == SessionId);
            if (session != null)
            {
                context.Sessions.Remove(session);
                context.SaveChanges();
            }

            if (sessionIsAdd)
            {
                context.Sessions.Add(new Session(SessionId));
                context.SessionTaxTypes.AddRange(new[]
                {
                    new SessionTaxType()
                    {
                        IdTaxType = context.TaxTypes.FirstOrDefault(tt => tt.IdTaxType == 5).IdTaxType,
                        SessionId = SessionId
                    },
                    new SessionTaxType()
                    {
                        IdTaxType = context.TaxTypes.FirstOrDefault(tt => tt.IdTaxType == 6).IdTaxType,
                        SessionId = SessionId
                    },
                    new SessionTaxType()
                    {
                        IdTaxType = context.TaxTypes.FirstOrDefault(tt => tt.IdTaxType == 7).IdTaxType,
                        SessionId = SessionId
                    },
                });
            }

            context.SaveChanges();
        }
    }
}
