using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataRepository;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class SettingsRepository: RepositoryBase<Settings>
    {
        public override Expression Expression
        {
            get { return Set.Where(e => e.Visible).Expression; }
        }

        protected override RepositoryDataContext GetContext()
        {
            return new TaxorgContext();
        }
    }
}
