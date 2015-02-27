using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TaxOrg.Infrastructure
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> query, string field,
            object value, string operation)
        {
            //Пример: Where(p => p.Name == value)

            var parameter = Expression.Parameter(query.ElementType, "p");

            MemberExpression memberAccess = null;
            foreach (var item in field.Split('.'))
            {
                memberAccess = Expression.Property((memberAccess ?? (parameter as Expression)), item);
            }

            var filter = Expression.Constant(Convert.ChangeType(value, memberAccess.Type));

            Expression body = null;
            LambdaExpression lambda = null;
            switch (operation)
            {
                case "eq":
                    body = Expression.Equal(memberAccess, filter);
                    lambda = Expression.Lambda(body, parameter);
                    break;
                case "ne":
                    body = Expression.NotEqual(memberAccess, filter);
                    lambda = Expression.Lambda(body, parameter);
                    break;
                case "cn":
                    body = Expression.Call(memberAccess, typeof(string).GetMethod("Contains"), Expression.Constant(value));
                    lambda = Expression.Lambda(body, parameter);
                    break;
                case "lt":
                    body = Expression.LessThan(memberAccess, filter);
                    lambda = Expression.Lambda(body, parameter);
                    break;
                case "le":
                    body = Expression.LessThanOrEqual(memberAccess, filter);
                    lambda = Expression.Lambda(body, parameter);
                    break;
                case "gt":
                    body = Expression.GreaterThan(memberAccess, filter);
                    lambda = Expression.Lambda(body, parameter);
                    break;
                case "ge":
                    body = Expression.GreaterThanOrEqual(memberAccess, filter);
                    lambda = Expression.Lambda(body, parameter);
                    break;
            }

            MethodCallExpression result = Expression.Call(typeof(Queryable), "Where", new[] { query.ElementType }, query.Expression, lambda);

            return query.Provider.CreateQuery<T>(result);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string field, string sortOrder)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var memberAccess = Expression.Property(parameter, field);
            var lambda = Expression.Lambda(memberAccess, parameter);

            var methodName = "OrderBy";
            if (sortOrder == "desc")
                methodName = "OrderByDescending";

            var method = Expression.Call(typeof (Queryable), methodName, new[] {query.ElementType, ((PropertyInfo)memberAccess.Member).PropertyType}, query.Expression, lambda);

            return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(method);
        }
    }
}