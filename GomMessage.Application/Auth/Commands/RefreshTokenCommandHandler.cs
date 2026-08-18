using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Entities;
using MediatR;

namespace GomMessage.Application.Auth.Commands
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _jwtGeneratorService;
        private readonly IUserRepository _userRepository;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository, 
            ITokenService jwtGeneratorService,
            IUserRepository userRepository
            )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtGeneratorService = jwtGeneratorService;
            _userRepository = userRepository;
        }
        public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var existToken = await _refreshTokenRepository.GetRefreshTokenByToken(request.RefreshToken,cancellationToken);
            if(existToken == null || !existToken.IsActive())
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }
            var user = await _userRepository.GetByIdAsync(existToken.UserId, cancellationToken);
            if (user == null) {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }
            var result = _jwtGeneratorService.GenerateToken(user.Id.ToString(),user.Email,user.Name);
            var newToken = RefreshToken.Create(user.Id, result.RefreshToken, result.RefreshTokenExpiresAt);
            return result;
        }
    }
}
