using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Samples
{
    public static class LinqExtensions
    {
        public static IOrderedQueryable<T> OrderByProperty<T>(
            this IQueryable<T> source, string propertyPath, bool ascending = true)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (string.IsNullOrWhiteSpace(propertyPath))
                throw new ArgumentException("Property path is required.", nameof(propertyPath));

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression body = parameter;

            foreach (var member in propertyPath.Split('.'))
            {
                var prop = body.Type.GetProperty(member)
                          ?? throw new InvalidOperationException(
                               $"Type '{body.Type.Name}' does not contain property '{member}'.");
                body = Expression.Property(body, prop);
            }

            var keySelector = Expression.Lambda(body, parameter);
            var methodName = ascending ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2);
            var generic = method.MakeGenericMethod(typeof(T), body.Type);
            return (IOrderedQueryable<T>)generic.Invoke(null, new object[] { source, keySelector });
        }
    }
}

