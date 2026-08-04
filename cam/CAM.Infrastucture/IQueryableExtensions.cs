using CAM.Contracts;
using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CAM.Infrastucture
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>[]> columnsMap , string propertyName = "ModificationDate")
        {
            if (string.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
            {

                var isAuditable = typeof(T).BaseType == typeof(AuditableEntity);
                if (isAuditable)
                {
                    string command = "OrderByDescending";
                    var type = typeof(T);
                    var property = type.GetProperty(propertyName);
                    var parameter = Expression.Parameter(type, "p");
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                    var resultExpression = Expression.Call(
                        typeof(Queryable),
                        command,
                        new Type[] { type, property.PropertyType },
                        query.Expression,
                        Expression.Quote(orderByExpression));
                    return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(resultExpression);
                }
                return query;
            }

            if (queryObj.IsSortAscending)
            {
                for (var index = 0; index < columnsMap[queryObj.SortBy].Length; index++)
                {
                    if (index > 0)
                    {

                        var expression = columnsMap[queryObj.SortBy][index];
                        query = ((IOrderedQueryable<T>)query).ThenBy(expression);
                    }
                    else
                    {

                        var expression = columnsMap[queryObj.SortBy][index];
                        query = query.OrderBy(expression);
                    }
                }
            }
            else
            {
                for (var index = 0; index < columnsMap[queryObj.SortBy].Length; index++)
                {
                    if (index > 0)
                    {

                        var expression = columnsMap[queryObj.SortBy][index];
                        query = ((IOrderedQueryable<T>)query).ThenByDescending(expression);
                    }
                    else
                    {

                        var expression = columnsMap[queryObj.SortBy][index];
                        query = query.OrderByDescending(expression);
                    }
                }
            }
            return query;
        }

         
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObj)
        {
            if (queryObj.Page <= 0)
                queryObj.Page = 1;

            if (queryObj.PageSize <= 0)
                queryObj.PageSize = query.Count();
            if (queryObj.PageSize == 0) return query;
            return query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);
        }
 

        public static List<T> ApplyPaginationList<T>(this List<T> list, IQueryObject queryObj)
        {
            if (queryObj.Page <= 0)
                queryObj.Page = 1;

            if (queryObj.PageSize <= 0)
                queryObj.PageSize = 10;

            if (queryObj.PageSize == 0)
                return list;

            int skip = (queryObj.Page - 1) * queryObj.PageSize;
            return list.Skip(skip).Take(queryObj.PageSize).ToList();
        }
    }
}
