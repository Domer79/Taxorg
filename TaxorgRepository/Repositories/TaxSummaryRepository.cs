using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    public class TaxSummaryRepository 
    {
        private static TaxSummaryRepository _repository;
        private readonly string _sessionId;
        private readonly TaxorgContext _context = new TaxorgContext();
        private static string _currentPeriod;
        private static string _prevPeriod;

        public TaxSummaryRepository(string sessionId)
        {
            _sessionId = sessionId;
        }

        public IQueryable<object> GetData()
        {
            var query = Context.Taxes.AsQueryable();
            query = query.Include(e => e.Organization);
            var intersectSource = Context.SessionTaxTypes.Where(stt => stt.SessionId == _sessionId).SelectMany(e => e.TaxType.Taxes);
            if (intersectSource.Any())
                query = query.Intersect(intersectSource);

            var groupedQuery = query
                .GroupBy(e => new {e.Organization, e.PeriodName})
                .Select(
                    e =>
                        new
                        {
                            e.Key.Organization.IdOrganization, 
                            e.Key.Organization.Inn, e.Key.Organization.Name, e.Key.Organization.ShortName, e.Key.Organization.Address,
                            Tax = e.Sum(t => t.TaxSum),
//                            e.Key.Organization,
                            e.Key.PeriodName,
                            PrevTax = Context.Taxes.Intersect(intersectSource)
                                    .Where(tax => tax.PeriodName == PrevPeriod && tax.Organization == e.Key.Organization)
                                    .Select(tax => tax == null ? 0 : tax.TaxSum)
                                    .Sum()
                        })
                .Where(e => e.PeriodName == CurrentPeriod);

            return groupedQuery;
        }

        private static string CurrentPeriod
        {
            get { return _currentPeriod ?? (_currentPeriod = TaxorgTools.GetCurrentPeriod().ToString()); }
        }

        private static string PrevPeriod
        {
            get { return _prevPeriod ?? (_prevPeriod = TaxorgTools.GetPrevPeriod().ToString()); }
        }

//        public List<TaxSummary> GeTaxSummaries(string sessionId, int offset, int count, bool isNotLoadSameTax)
//        {
//            
//        }

        public static TaxSummaryRepository GetRepository(string sessionId)
        {
            return _repository ?? (_repository = new TaxSummaryRepository(sessionId));
        }

        public TaxorgContext Context
        {
            get { return _context; }
        }
    }
}
