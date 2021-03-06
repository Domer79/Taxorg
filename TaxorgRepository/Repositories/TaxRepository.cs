using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Mime;
using SystemTools;
using SystemTools.Extensions;
using SystemTools.WebTools.Infrastructure;
using DataRepository;
using DataRepository.Exceptions;
using DataRepository.Infrastructure;
using SqlClr;
using TaxorgRepository.Exceptions;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class TaxRepository : RepositoryBase<Tax>
    {
        private static TaxRepository _repository;

        public static TaxRepository Repository
        {
            get { return _repository ?? (_repository = new TaxRepository()); }
        }

        private Tax GetTax(Organization organization, TaxType taxType, string periodName)
        {
            var tax = this.SingleOrDefault(e => e.IdOrganization == organization.IdOrganization && e.IdTaxType == taxType.IdTaxType && e.PeriodName == periodName);

            if (tax == null)
            {
                tax = Create();
                tax.IdOrganization = organization.IdOrganization;
                tax.IdTaxType = taxType.IdTaxType;
                tax.Period = periodName;
            }

            return tax;
        }

        public void SaveTaxToDb(string inn, string taxCode, string taxName, DateTime dateLoad, decimal taxSum)
        {
            var db = Context.Database;
            try
            {
                db.ExecuteSqlCommand("exec SaveTax @p0, @p1, @p2, @p3, @p4", inn, taxCode, taxName, ((YearMonth)dateLoad).ToString(), taxSum);
            }
            catch (Exception e)
            {
                e.SaveError();
                throw new SaveTaxException(e.Message);
            }
        }

        public void DeletePeriod(DateTime period)
        {
            var db = Context.Database;
            try
            {
                db.ExecuteSqlCommand("delete from Tax where period = @p0", ((YearMonth)period).ToString());
            }
            catch (Exception e)
            {
                e.SaveError();
                throw new SaveTaxException(e.Message);
            }
        }

        protected override RepositoryDataContext GetContext()
        {
            return new TaxorgContext();
        }
    }
}