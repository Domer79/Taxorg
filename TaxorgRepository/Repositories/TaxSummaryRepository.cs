using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class TaxSummaryRepository : RepositoryBase<TaxSummary>
    {
        private static TaxSummaryRepository _repository;

        public override void InsertOrUpdate(TaxSummary item)
        {
//            var org = Context.Organization.SingleOrDefault(o => o.IdOrganization == item.IdOrganization);
//            if (org != null)
//            {
//                org.Name = item.Name;
//                org.ShortName = item.ShortName;
//                Context.Entry(org).State = EntityState.Modified;
//                return;
//            }
//
//            org = new Organization();
//            org.Name = item.Name;
//            org.ShortName = item.ShortName;
//            org.Inn = item.Inn;
//            org.Address = item.Address;
//
//            Context.Organization.Add(org);

            throw new NotImplementedException();
        }

        public override void Delete(TaxSummary item)
        {
            throw new NotImplementedException();
        }

        public static TaxSummaryRepository Repository
        {
            get { return _repository ?? (_repository = new TaxSummaryRepository()); }
        }
    }
}
