using System;
using System.Collections.Generic;
using System.Linq;
using SystemTools.Extensions;
using SystemTools.WebTools.Infrastructure;

namespace SystemTools.WebTools.Helpers
{
    public static class ControllerHelper
    {
        public static object GetData<TEntity>(GridSettings grid, IQueryable<TEntity> repository, string keyName) where TEntity : class
        {
            try
            {
                return GetDataNotSafe(grid, repository, keyName);
            }
            catch (Exception e)
            {
                e.SaveError();
                throw;
            }
        }

        private static object GetDataNotSafe<TEntity>(GridSettings grid, IQueryable<TEntity> repository, string keyName) where TEntity : class
        {
            var query = repository;

            var count = query.Count();
            var totalPage = (int) Math.Ceiling((double) count/grid.PageSize);
            totalPage = totalPage == 0 ? 1 : totalPage;
            grid.PageIndex = (grid.PageIndex > totalPage) ? totalPage : grid.PageIndex;
            query = query.SetFilterToQuery(grid.Where, string.IsNullOrEmpty(grid.SortColumn) ? keyName : grid.SortColumn,
                grid.SortOrder, (grid.PageIndex - 1)*grid.PageSize, grid.PageSize);

            var data = query.ToList();

            var jsonResult = new
            {
                rows = data,
                totalPages = totalPage,
                records = count,
                pageIndex = 1
            };

            return jsonResult;
        }

        public static IQueryable<TEntity> SetFilterToQuery<TEntity>(this IQueryable<TEntity> repository, Filter filter, 
            string sortColumn, string sortOrder, int offset, int count)
            where TEntity : class
        {
            var query = repository;

            //Поиск
            if (filter != null)
            {
                //AND
                if (filter.groupOp == "AND")
                {
                    query = filter.rules.Aggregate(query,
                        (current, rule) => current.Where(rule.field, rule.data, rule.op));
                }
                else
                {
                    var temp = new List<TEntity>();
                    temp = filter.rules.Select(rule => query.Where(rule.field, rule.data, rule.op))
                        .Aggregate(temp, (current, t) => (List<TEntity>) current.Concat(t));
                    query = (IQueryable<TEntity>) temp.Distinct();
                }
            }

//            query = query.OrderBy(string.IsNullOrEmpty(grid.SortColumn) ? sortColumn : grid.SortColumn, grid.SortOrder);
            query = query.OrderBy(sortColumn, sortOrder);
            query = query.Skip(offset).Take(count);
//                .Skip((grid.PageIndex - 1)*grid.PageSize)
//                .Take(grid.PageSize);

            return query;
        }

        public static string GetActionPath(string controller, string action)
        {
            return string.Format("{0}/{1}", controller, action);
        }
    }
}