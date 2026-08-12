using GomMessage.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Tenants.Dtos
{
    public sealed record TenantResponse(Guid Id, string Name, string Slug, string settings);
    public class ListTenantResponse : BaseResponseDto<TenantResponse>;
}
