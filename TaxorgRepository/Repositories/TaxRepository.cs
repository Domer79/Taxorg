using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
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

        public void SaveTaxToDb(string inn, string taxCode, DateTime dateLoad, decimal taxSum)
        {
            var db = Context.Database;
            try
            {
                db.ExecuteSqlCommand("exec SaveTax @p0, @p1, @p2, @p3", inn, taxCode, ((YearMonth)dateLoad).ToString(), taxSum);
            }
            catch (Exception e)
            {
                ErrorLog.SaveError(e);
                throw new SaveTaxException(e.Message);
            }
        }
    }
}