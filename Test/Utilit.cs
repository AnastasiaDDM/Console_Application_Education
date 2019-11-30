using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Test
{
    static class Utilit
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> q, string SortField, string ascdesc)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = ascdesc == "asc" ? "OrderBy" : "OrderByDescending";
            System.Type[] types = new System.Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}