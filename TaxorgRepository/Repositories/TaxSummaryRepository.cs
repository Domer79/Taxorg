using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SystemTools.WebTools.Helpers;
using SystemTools.WebTools.Infrastructure;
using SqlClr;
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
        private readonly object _data;

        public TaxSummaryRepository(string sessionId, GridSettings gridSettings)
        {
            _sessionId = sessionId;
            _sessionId = "qwertyuiop";//TODO: Потом удалить
            _data = GetData(gridSettings);
        }

        private object GetData(GridSettings gridSettings)
        {
            var query = Context.Taxes.AsQueryable();
            query = query.Include(e => e.Organization);
            var intersectSource = Context.SessionTaxTypes.Where(stt => stt.SessionId == _sessionId).SelectMany(e => e.TaxType.Taxes);
            if (intersectSource.Any())
                query = query.Intersect(intersectSource);

            var taxByOrganizationQuery = query
                .GroupBy(e => new {e.Organization, e.PeriodName})
                .Select(
                    e =>
                        new
                        {
                            e.Key.Organization.IdOrganization, 
                            e.Key.Organization.Inn, 
                            e.Key.Organization.Name, 
                            e.Key.Organization.ShortName, 
                            e.Key.Organization.Address,
                            Tax = e.Sum(t => t.TaxSum),
                            e.Key.PeriodName,
                            PrevTax = Math.Abs(Context.Taxes.Intersect(intersectSource) //TODO: Продумать правильное использование Intersect
                                    .Where(tax => tax.PeriodName == PrevPeriod && tax.Organization == e.Key.Organization)
                                    .Select(tax => tax.TaxSum)
                                    .Sum() ?? 0)
                        })
                .Where(e => e.PeriodName == CurrentPeriod);

            Count = taxByOrganizationQuery.Count();
            var totalPage = (int) Math.Ceiling((double) Count/gridSettings.PageSize);
            totalPage = totalPage == 0 ? 1 : totalPage;
            gridSettings.PageIndex = (gridSettings.PageIndex > totalPage) ? totalPage : gridSettings.PageIndex;

            taxByOrganizationQuery = taxByOrganizationQuery.SetFilterToQuery(gridSettings.Where,
                string.IsNullOrEmpty(gridSettings.SortColumn) ? "Inn" : gridSettings.SortColumn, gridSettings.SortOrder,
                (gridSettings.PageIndex - 1)*gridSettings.PageSize, gridSettings.PageSize);

            var data = taxByOrganizationQuery.ToList().Select(e => new
            {
                e.IdOrganization,
                e.Inn,
                e.Name,
                e.ShortName,
                e.Address,
                Tax = Math.Abs(e.Tax.Value),
                TaxDebitKredit = e.Tax > 0 ? string.Format("+{0}", e.Tax.Value.ToString("C")) : Math.Abs(e.Tax.Value).ToString("C"),
                e.PeriodName,
                PrevTax = Math.Abs(e.PrevTax).ToString("C"),
                Delta = (e.Tax.Value - Math.Abs(e.PrevTax)).ToString("C")
            });

            var jsonResult = new
            {
                rows = data,
                totalPages = totalPage,
                records = Count,
                pageIndex = 1
            };

            return jsonResult;
        }

        private static string CurrentPeriod
        {
            get { return _currentPeriod ?? (_currentPeriod = TaxorgTools.GetCurrentPeriod().ToString()); }
        }

        private static string PrevPeriod
        {
            get { return _prevPeriod ?? (_prevPeriod = TaxorgTools.GetPrevPeriod().ToString()); }
        }

        public static TaxSummaryRepository GetRepository(string sessionId, GridSettings gridSettings)
        {
            return _repository ?? (_repository = new TaxSummaryRepository(sessionId, gridSettings));
        }

        private TaxorgContext Context
        {
            get { return _context; }
        }

        public int Count { get; private set; }

        public object Data
        {
            get { return _data; }
        }
    }
}
