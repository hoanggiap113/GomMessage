

using GomMessage.Domain.Constants;

namespace GomMessage.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FilterAttribute : Attribute
    {
        public FilterOperator Operator { get; }
        public string? TargetProperty { get; set; }
        public FilterAttribute(FilterOperator op = FilterOperator.Equal)
        {
            Operator = op;
        }
    }
}
