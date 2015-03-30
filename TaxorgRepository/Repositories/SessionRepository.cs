using System;
using System.Data.Entity;
using DataRepository;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class SessionRepository: RepositoryBase<Session>
    {
        public override void InsertOrUpdate(Session item)
        {
            Set.Add(item);
        }

        protected override DbContext GetContext()
        {
            return new TaxorgContext();
        }
    }
}
