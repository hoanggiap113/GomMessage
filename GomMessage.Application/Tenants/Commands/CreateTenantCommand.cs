using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Tenants.Commands
{
    public sealed record CreateTenantCommand(string Name, string? Slug, string Telephone, string? Settings) : IRequest;

    public class CreatetenantCommandValidatior : AbstractValidator<CreateTenantCommand>
    {
        public CreatetenantCommandValidatior()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tenant name must exist");
            When(x => !string.IsNullOrWhiteSpace(x.Slug), () =>
            {
                RuleFor(x => x.Slug)
                    .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
                    .WithMessage("\r\nThe slug must only contain lowercase letters, numbers, and hyphens; it cannot contain spaces..")
                    .MaximumLength(100)
                    .WithMessage("\r\nThe slug must not exceed 100 characters.");
            });
            RuleFor(x => x.Telephone)
                .NotEmpty()
                .WithMessage("Telephone must not be null");
        }
    }
    
}
