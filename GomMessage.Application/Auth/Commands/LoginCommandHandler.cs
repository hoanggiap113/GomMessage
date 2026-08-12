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
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IJwtService _jwtGeneratorService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoginCommandHandler(
            IUserRepository userRepository, 
            IHashPasswordService hashPasswordService,
            IJwtService jwtGeneratorService, 
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork
            )
        {
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
            _jwtGeneratorService = jwtGeneratorService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
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
                _refreshTokenRepository.RevokeRefreshToken(existingToken);
            }

            var response = _jwtGeneratorService.GenerateToken(user.Id.ToString(), user.Email, user.Name);

            var newRefreshToken = RefreshToken.Create(
                userId: user.Id,
                tokenHash: response.RefreshToken,
                expiresAt: response.RefreshTokenExpiresAt
            );

            _refreshTokenRepository.CreateToken(newRefreshToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken );
            return response;
        }
    }
}
