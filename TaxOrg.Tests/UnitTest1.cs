using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemTools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlClr;
using TaxOrg.Controllers;
using TaxOrg.Tools;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;
using TaxorgRepository.Tools;
using SystemTools;
using Organization = TaxOrg.Tools.Organization;

namespace TaxOrg.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            ApplicationCustomizer.ConnectionString =
                "data source=cito1;initial catalog=Taxorg;User Id=developer;Password=sppdeveloper;MultipleActiveResultSets=True;App=EntityFramework";
        }

        [TestMethod]
        public void SqlClrYearMonthTest()
        {
            var yearMonth = new YearMonth() {Year = 2015, Month = 6};

            for (int i = 0; i < 20; i++)
            {
                Debug.WriteLine(yearMonth--);
            }

            Debug.WriteLine(yearMonth);
        }

        [TestMethod]
        public void SqlClrYearMonthSetTest()
        {
//            var yearMonth = new YearMonth();
            var yearMonth = YearMonth.Parse("01.2015");
            var expectedYearMonth = new YearMonth{Month = 1, Year = 2015};
            Assert.AreEqual(expectedYearMonth, yearMonth);
            Assert.AreEqual(expectedYearMonth.ToString(), "01.2015");
            Debug.WriteLine(expectedYearMonth);
        }

        [TestMethod]
        public void FileReadFromDbTest()
        {
            FileSystemRepository.FileDataName = "data";
            FileSystemRepository.FileTableName = "FsFile";

            using (var fs = new FileStream(@"c:\Users\Domer\Documents\excel1.xlsx", FileMode.Create))
            {
                using (var stream = FileSystemRepository.GetStreamFromData(9))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.DeCompress(fs);
                }
            }
        }

        [TestMethod]
        public void ErrorSaveTest()
        {
            ApplicationCustomizer.ConnectionString = "Data Source=.;Integrated Security=True;Initial Catalog=Taxorg";
            ApplicationCustomizer.RegisterErrorLog(ErrorLog.SaveError);

            try
            {
                throw new NotImplementedException("Тестовый метод не реализации");
            }
            catch (Exception e)
            {
                e.SaveError();
            }
        }

        [TestMethod]
        public void TextFileEncodingTest()
        {
            var fileName = @"c:\Users\User\Documents\Формат данных в Excel.csv";
            using (var sr = new StreamReader(fileName, Encoding.GetEncoding(866), false))
            {
                var buffer = new byte [sr.BaseStream.Length];
                sr.BaseStream.Read(buffer, 0, buffer.Length);
                var encodingString = Encoding.GetEncoding(1251).GetString(buffer);
                Debug.WriteLine(encodingString);
            }
        }

        [TestMethod]
        public void CharToCodeTest()
        {
            var b = (byte) 's';
            Debug.WriteLine(b);
        }

        [TestMethod]
        public void CsvReaderTest()
        {
            using (var reader = new CsvReader<Organization>(@"c:\Users\Domer\Documents\Формат данных в Excel.csv"))
            {
                foreach (var organization in reader)
                {
                    Debug.WriteLine(organization);
                }
            }
        }

        [TestMethod]
        public void EntityGetMemberAccessTest()
        {
            ApplicationCustomizer.ConnectionString = "Data Source=.;Integrated Security=True;Initial Catalog=Taxorg";
            var context = new TaxorgContext(ApplicationCustomizer.ConnectionString);
            var entityInfo = new EntityInfo<Bug>(context);
            Expression<Func<Bug, int>> actualExpression = p => p.IdBug;

            Assert.AreEqual(entityInfo.GetMemberAccess<int>(), actualExpression);
        }

        [TestMethod]
        public void SliceRepositoryTest()
        {
            ApplicationCustomizer.ConnectionString = "Data Source=.;Integrated Security=True;Initial Catalog=Taxorg";
            var repo = new SliceRepository(7816);
            foreach (var sliceTax in repo)
            {
                Debug.WriteLine(sliceTax.Period);
            }
        }

        [TestMethod]
        public void SliceTaxDebitKreditFieldTest()
        {
            ApplicationCustomizer.ConnectionString = "Data Source=.;Integrated Security=True;Initial Catalog=Taxorg";
            var repo = new SliceRepository(1989);

            var query = repo.Where(e => e.TaxDebitKredit.Contains("+"));

            foreach (var tax in query)
            {
                Debug.WriteLine(tax.TaxDebitKredit);
            }
        }

        [TestMethod]
        public void AppVersionTest()
        {
            Assert.AreEqual("1.1.0.0", ApplicationCustomizer.AppVersion);
        }

        [TestMethod]
        public void YearMonthDifferenceTest()
        {
            var period1 = new YearMonth("01.2016");
            var period2 = new YearMonth("12.2015");

            var actual = period1 - period2;
            Debug.WriteLine(actual);
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
                throw new ArgumentNullException("Test exception");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        [TestMethod]
        public void DbContextSqlQueryTest()
        {
            var context = new TaxorgContext();
//            var query = context.Database.SqlQuery<TaxSummary>("select top(5) idOrganizations, inn, name, shortName, addr as Address, tax, taxDebitKredit, prevTax, periodName from TaxSummary");
            var query = context.TaxSummaries;

            foreach (var taxSummary in query)
            {
                Debug.WriteLine(taxSummary.Name, taxSummary.Tax);
            }
        }

        [TestMethod]
        public void GetErrorMessageTest()
        {
            try
            {
                throw new Exception("Exception 1");
            }
            catch (Exception e1)
            {
                try
                {
                    throw new Exception("Exception 2", e1);
                }
                catch (Exception e2)
                {
                    try
                    {
                        throw new Exception("Exception 3", e2);
                    }
                    catch (Exception e3)
                    {
                        Debug.WriteLine(e3.GetErrorMessage());
                    }
                }
            }
        }
    }
}
