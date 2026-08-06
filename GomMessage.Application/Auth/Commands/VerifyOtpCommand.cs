using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Auth.Commands
{
    public sealed record VerifyOtpCommand(string Email, string Otp) : IRequest<string>
    {
    }
}
