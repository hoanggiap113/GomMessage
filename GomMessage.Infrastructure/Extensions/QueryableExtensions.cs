using GomMessage.Application.Common.Attributes;
using GomMessage.Domain.Constants;
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

                var filterAttr = prop.GetCustomAttribute<FilterAttribute>();
                var targetPropName = filterAttr?.TargetProperty ?? prop.Name;
                var filterOperator = filterAttr?.Operator ?? (prop.PropertyType == typeof(string) ? FilterOperator.Contains : FilterOperator.Equal);

                // Find appropiate property in the entity type, ignoring case
                var entityProp = entityType.GetProperty(targetPropName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (entityProp == null) continue;


                var left = Expression.Property(parameter, entityProp);
                var targetType = entityProp.PropertyType;
                var underlyingTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                //Type safe conversion
                object? safeValue;
                try
                {
                    if (underlyingTargetType.IsEnum)
                    {
                        safeValue = Enum.Parse(underlyingTargetType, filterValue.ToString()!);
                    }
                    else if (underlyingTargetType == typeof(Guid))
                    {
                        safeValue = Guid.Parse(filterValue.ToString()!);
                    }
                    else
                    {
                        safeValue = Convert.ChangeType(filterValue, underlyingTargetType);
                    }
                }
                catch
                {
                    continue; 
                }

                var right = Expression.Constant(safeValue, underlyingTargetType);
                Expression rightConverted = targetType != underlyingTargetType
                    ? Expression.Convert(right, targetType)
                    : (Expression)right;

                var comparison = BuildComparisonExpression(left, rightConverted, filterOperator, underlyingTargetType);
                if (comparison == null) continue;

                combinedExpression = combinedExpression == null
                    ? comparison
                    : Expression.AndAlso(combinedExpression, comparison);
            
            }
            if (combinedExpression == null) return query;

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            return query.Where(lambda);
        }
        private static Expression? BuildComparisonExpression(Expression left, Expression right, FilterOperator op, Type underlyingType)
        {
            return op switch
            {
                FilterOperator.Equal => Expression.Equal(left, right),
                FilterOperator.NotEqual => Expression.NotEqual(left, right),
                FilterOperator.GreaterThan => Expression.GreaterThan(left, right),
                FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
                FilterOperator.LessThan => Expression.LessThan(left, right),
                FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
                FilterOperator.Contains when underlyingType == typeof(string) => BuildStringMethod(left, right, nameof(string.Contains)),
                FilterOperator.StartsWith when underlyingType == typeof(string) => BuildStringMethod(left, right, nameof(string.StartsWith)),
                FilterOperator.EndsWith when underlyingType == typeof(string) => BuildStringMethod(left, right, nameof(string.EndsWith)),
                _ => Expression.Equal(left, right)
            };
        }
        private static Expression BuildStringMethod(Expression left, Expression right, string methodName)
        {
            var method = typeof(string).GetMethod(methodName, new[] { typeof(string) })!;
            return Expression.Call(left, method, right);
        }
    }
}
