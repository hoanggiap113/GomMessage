using FluentValidation;
using GomMessage.Domain.Entities.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Tenants.Commands
{
    public sealed record InviteMemberCommand(Guid TenantId, string Email, TenantRole Role) : IRequest;
    public class InviteMemberCommandValidator : AbstractValidator<InviteMemberCommand>
    {
        public InviteMemberCommandValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Role is not valid")
                .NotEqual(TenantRole.Owner).WithMessage("Can not invite member with role Owner");
        }
    }
}
