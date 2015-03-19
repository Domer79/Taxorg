using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class SessionRepository: RepositoryBase<Session>
    {
        public override void InsertOrUpdate(Session item)
        {
            Set.Add(item);
        }
    }
}
