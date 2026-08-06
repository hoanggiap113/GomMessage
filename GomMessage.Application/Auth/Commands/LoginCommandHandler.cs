using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using GomMessage.Domain.Common;
using GomMessage.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Auth.Commands
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IJwtGeneratorService _jwtGeneratorService;
        
        public LoginCommandHandler(IUserRepository userRepository, IHashPasswordService hashPasswordService, IJwtGeneratorService jwtGeneratorService)
        {
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
            _jwtGeneratorService = jwtGeneratorService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email,cancellationToken);
            if(user == null)
            {
                throw new NotFoundException(ErrorCode.UserNotFound);
            }
            if(!_hashPasswordService.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new DomainException(ErrorCode.InvalidCredentials);
            }
           var response = _jwtGeneratorService.GenerateToken(user.Id.ToString(), user.Email, user.Name);
           return response;
        }
    }
}
