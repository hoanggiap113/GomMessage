using GomMessage.Application.Tenants.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Tenants.Queries
{
    public sealed record GetTenantsQuery(int Page, int Limit) : IRequest<ListTenantResponse>;
    
}
