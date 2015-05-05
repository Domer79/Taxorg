using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class Repository<TEntity> : RepositoryBase<TEntity> 
        where TEntity : ModelBase
    {
        protected override RepositoryDataContext GetContext()
        {
            return new TaxorgContext();
        }
    }
}
