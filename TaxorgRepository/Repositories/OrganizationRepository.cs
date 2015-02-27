using System.Linq;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class OrganizationRepository : RepositoryBase<Organization>
    {
        private static OrganizationRepository _repository;

        public static OrganizationRepository Repository
        {
            get { return _repository ?? (_repository = new OrganizationRepository()); }
        }

        public static Organization GetOrganization(string inn)
        {
            return Repository[inn];
        }

        public static Organization GetOrganization(int idOrganization)
        {
            return Repository[idOrganization];
        }

        public Organization this[string inn]
        {
            get
            {
                var organization = Set.SingleOrDefault(e => e.Inn == inn);
                return organization;
            }
        }

        public Organization this[int idOrganization]
        {
            get
            {
                var organization = Set.SingleOrDefault(e => e.IdOrganization == idOrganization);
                return organization;
            }
        }

        public static Organization CreateOrganization(string inn)
        {
            var organization = Repository.Create();
            organization.Inn = inn;
            Repository.InsertOrUpdate(organization);
            Repository.SaveChanges();
            return organization;
        }
    }
}
