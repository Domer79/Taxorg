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

        public void LoadTax(Organization organization, TaxType taxType, DateTime date, decimal taxSum)
        {
            var tax = Repository.GetTax(organization, taxType, date);
            tax.TaxSum = taxSum;
        }

        private Tax GetTax(Organization organization, TaxType taxType, DateTime date)
        {
            var tax = this.SingleOrDefault(e => e.IdOrganization == organization.IdOrganization && e.IdTaxType == taxType.IdTaxType && e.Period == date);

            if (tax == null)
            {
                tax = Create();
                tax.IdOrganization = organization.IdOrganization;
                tax.IdTaxType = taxType.IdTaxType;
                tax.Period = date;
                InsertOrUpdate(tax);
            }

            return tax;
        }

        public bool SaveTax(string inn, string taxCode, DateTime dateLoad, decimal taxSum, out string errorStr)
        {
            errorStr = string.Empty;
            try
            {
                var contextOrganization = OrganizationRepository.GetOrganization(inn) ?? OrganizationRepository.CreateOrganization(inn);
                var taxType = TaxTypeRepository.GetTaxType(taxCode) ?? TaxTypeRepository.Create(taxCode);
                LoadTax(contextOrganization, taxType, dateLoad, taxSum);

                var isError = false;
                foreach (var item in Context.GetValidationErrors())
                {
                    errorStr = item.ValidationErrors.Aggregate("",
                        (accumulate, error) => accumulate + error.ErrorMessage);
                    isError = true;
                }

                return !isError;
            }
            catch (Exception e)
            {
                errorStr = e.Message;
                return false;
            }
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