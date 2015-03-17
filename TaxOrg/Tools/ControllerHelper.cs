using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemTools.Extensions;
using TaxOrg.Infrastructure;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;

namespace TaxOrg.Tools
{
    public class ControllerHelper
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
            IQueryable<TEntity> query = repository.AsQueryable();

            //Поиск
            if (grid.IsSearch)
            {
                //AND
                if (grid.Where.groupOp == "AND")
                {
                    query = grid.Where.rules.Aggregate(query,
                        (current, rule) => current.Where(rule.field, rule.data, rule.op));
                }
                else
                {
                    var temp = new List<TEntity>();
                    temp = grid.Where.rules.Select(rule => query.Where(rule.field, rule.data, rule.op))
                        .Aggregate(temp, (current, t) => (List<TEntity>) current.Concat(t));
                    query = (IQueryable<TEntity>) temp.Distinct();
                }
            }

//            query = query.OrderByDescending(repository.Context.GetExpression<TEntity>(grid.SortColumn));
//            query.Where();
            query = query.OrderBy(string.IsNullOrEmpty(grid.SortColumn) ? keyName : grid.SortColumn, grid.SortOrder);

            int count = query.Count();
            var totalPage = (int) Math.Ceiling((double) count/grid.PageSize);
            totalPage = totalPage == 0 ? 1 : totalPage;
            grid.PageIndex = (grid.PageIndex > totalPage) ? totalPage : grid.PageIndex;
            List<TEntity> data = query.Skip((grid.PageIndex - 1)*grid.PageSize).Take(grid.PageSize).ToList();
            //            var data = query.ToList();

            var jsonResult = new
            {
                rows = data,
                totalPages = totalPage,
                records = count,
                pageIndex = 1
            };

            return jsonResult;
        }
    }
}