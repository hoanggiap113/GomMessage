using GomMessage.Application.Auth.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Auth.Commands
{
    public sealed record RegisterCommand(string Email, string Password, string Name, string Telephone) : IRequest<RegisterUserResponse>;
}
