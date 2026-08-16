using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyDynamicFilter<T>(this IQueryable<T> query, object? filter)
        {
            if (filter == null) return query;

            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "x");

            Expression? combinedExpression = null;

            //Get all properties of the filter object
            var filterProps = filter.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var prop in filterProps)
            {
                var filterValue = prop.GetValue(filter);
                if (filterValue == null) continue;

                //Check entity has duplicate property
                var entityProp = entityType.GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (entityProp == null) continue;

                var left = Expression.Property(parameter, entityProp);

                var targetType = entityProp.PropertyType;
                var right = Expression.Constant(
                    Convert.ChangeType(filterValue, Nullable.GetUnderlyingType(targetType) ?? targetType),
                    targetType
                );
                Expression comparison = Expression.Equal(left, right);

                combinedExpression = combinedExpression == null
                    ? comparison
                    : Expression.AndAlso(combinedExpression, comparison);
            }

            if (combinedExpression == null) return query;

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return query.Where(lambda);
        }
    }
}
