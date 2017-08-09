using System;
using System.Linq;
using System.Linq.Expressions;

namespace App.Core.Utils
{
    public static class SortUtils
    {
        private static readonly Type QueryableType = typeof(Queryable);

        public static IOrderedQueryable<T> OrderDescending<T>(this IQueryable<T> query, string propertyName)
        {
            return ApplyOrdering(query, propertyName, false, true);
        }

        public static IOrderedQueryable<T> OrderAscending<T>(this IQueryable<T> query, string propertyName)
        {
            return ApplyOrdering(query, propertyName, true, true);
        }

        private static IOrderedQueryable<T> ApplyOrdering<T>(
            IQueryable<T> query,
            string propertyName,
            bool sortAscending,
            bool useReflection)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            var selector = Expression.Lambda<Func<T, object>>(propAsObject, parameter);

            if (!useReflection)
                return sortAscending ? query.OrderBy(selector) : query.OrderByDescending(selector);

            var body = selector.Body as UnaryExpression;
            if (body == null)
                return sortAscending ? query.OrderBy(selector) : query.OrderByDescending(selector);


            if (body.Operand is MemberExpression keyExpr)
                return (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        QueryableType,
                        sortAscending ? "OrderBy" : "OrderByDescending",
                        new[] { typeof(T), keyExpr.Type },
                        query.Expression,
                        Expression.Lambda(keyExpr, selector.Parameters)));

            return sortAscending ? query.OrderBy(selector) : query.OrderByDescending(selector);
        }
    }
}