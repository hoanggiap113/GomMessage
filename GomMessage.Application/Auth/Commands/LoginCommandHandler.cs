using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Common;
using GomMessage.Domain.Entities;
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
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginCommandHandler(
            IUserRepository userRepository, 
            IHashPasswordService hashPasswordService,
            IJwtGeneratorService jwtGeneratorService, 
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
            _jwtGeneratorService = jwtGeneratorService;
            _refreshTokenRepository = refreshTokenRepository;
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
            var existingToken = await _refreshTokenRepository.GetByUserId(user.Id, cancellationToken);
            if (existingToken != null)
            {
                await _refreshTokenRepository.RevokeRefreshToken(existingToken.TokenHash, cancellationToken);
            }

            var response = _jwtGeneratorService.GenerateToken(user.Id.ToString(), user.Email, user.Name);
            await _refreshTokenRepository.CreateTokenAsync(
                response.RefreshToken,
                user.Id,
                response.RefreshTokenExpiresAt,
                cancellationToken);
            return response;
        }
    }
}
