using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class SettingsRepository: RepositoryBase<Settings>
    {
        public override Expression Expression
        {
            get { return Set.Where(e => e.Visible).Expression; }
        }
    }
}
