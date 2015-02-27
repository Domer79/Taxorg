using System.Linq;
using TaxorgRepository.Interfaces;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class TaxTypeRepository : RepositoryBase<TaxType>
    {
        private static TaxTypeRepository _repository;

        public static TaxTypeRepository Repository
        {
            get
            {
                return _repository ?? (_repository = new TaxTypeRepository());
            }
        }

        public static TaxType GetTaxType(string taxCode)
        {
            return Repository[taxCode];
        }

        protected TaxType this[string taxCode]
        {
            get
            {
                var taxType = this.SingleOrDefault(e => e.Code == taxCode);
                return taxType;
            }
        }

        public TaxType CreateTaxType(string taxCode)
        {
            var taxType = Create();
            taxType.Code = taxCode;
            InsertOrUpdate(taxType);

            SaveChanges();
            return taxType;
        }

        public static TaxType Create(string taxCode)
        {
            return Repository.CreateTaxType(taxCode);
        }

        public TaxType AddTaxType(string taxCode, string taxName)
        {
            var taxType = Repository.Create();
            taxType.Code = taxCode;
            taxType.Name = taxName;
            Repository.InsertOrUpdate(taxType);
            return taxType;
        }
    }
}