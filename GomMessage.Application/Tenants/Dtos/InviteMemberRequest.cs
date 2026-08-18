using GomMessage.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Tenants.Dtos
{
    public sealed record InviteMemberRequest(string Email, TenantRole Role);
}
