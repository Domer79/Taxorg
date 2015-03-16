using System;
using System.Linq;
using System.Linq.Expressions;
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
            throw new NotImplementedException();
        }

        public static TaxSummaryRepository Repository
        {
            get { return _repository ?? (_repository = new TaxSummaryRepository()); }
        }

        public override Expression Expression
        {
            get
            {
                if (TaxorgTools.IsNotSameTaxLoad)
                    return Set.Where(e => e.Tax != e.PrevTax).Expression;

                return base.Expression;
            }
        }
    }
}
