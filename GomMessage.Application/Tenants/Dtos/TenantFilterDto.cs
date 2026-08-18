using GomMessage.Application.Common.Attributes;
using GomMessage.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Tenants.Dtos
{
    public class TenantFilterDto
    {
        public string? Name { get; set; }
        [Filter(FilterOperator.Equal, TargetProperty = "UserId")]
        public string? UserId { get; set; }

        [Filter(FilterOperator.GreaterThanOrEqual, TargetProperty = "CreatedAt")]
        public DateTime? FromDate { get; set; }
        [Filter(FilterOperator.LessThanOrEqual, TargetProperty = "CreatedAt")]
        public DateTime? ToDate { get; set; }
    }
}
