using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataRepository;
using TaxorgRepository.Exceptions;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public sealed class SliceRepository : RepositoryBase<SliceTax>
    {
        public SliceRepository(string innOrganization)
            : this(OrganizationRepository.GetOrganization(innOrganization))
        {

        }

        public SliceRepository(int idOrganization)
            : this(OrganizationRepository.GetOrganization(idOrganization))
        {
        }
        

        public SliceRepository(Organization organization)
        {
            if (organization == null) 
                throw new ArgumentNullException("organization");

            _organization = organization;
        }

        private readonly Organization _organization;
        public override void InsertOrUpdate(SliceTax item)
        {
            if (item.IdOrganization == default(int))
                throw new SliceTaxInsertOrUpdateException("Не верный идентификатор организации IdOrganization");

            var taxType = TaxTypeRepository.Repository.SingleOrDefault(e => e.IdTaxType == item.IdTaxType) ?? TaxTypeRepository.Repository.Create();

            taxType.Code = item.TaxCode;
            taxType.Name = item.TaxName;
            
            TaxTypeRepository.Repository.InsertOrUpdate(taxType);
            TaxTypeRepository.Repository.SaveChanges();

            item.IdTaxType = taxType.IdTaxType;

            var tax = TaxRepository.Repository.SingleOrDefault(e => e.IdTax == item.IdTax) ?? TaxRepository.Repository.Create(); ;

            tax.IdTaxType = item.IdTaxType;
            tax.IdOrganization = item.IdOrganization;
            tax.TaxSum = item.TaxSum;
            tax.Period = item.Period;

            TaxRepository.Repository.InsertOrUpdate(tax);
            TaxRepository.Repository.SaveChanges();
        }

        public override Expression Expression
        {
            get
            {
                return Set.Where(e => e.IdOrganization == _organization.IdOrganization).Expression;
            }
        }

        protected override DbContext GetContext()
        {
            return new TaxorgContext();
        }
    }
}