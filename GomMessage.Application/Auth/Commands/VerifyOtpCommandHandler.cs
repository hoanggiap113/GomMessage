using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Entities;
using GomMessage.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GomMessage.Application.Auth.Commands
{
    public sealed class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, string>
    {
        private const int MaxFailedAttempts = 4;

        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;

        public VerifyOtpCommandHandler(
            IUserRepository userRepository,
            ICacheService cacheService
            )
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
        }

        public async Task<string> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            {
                throw new DomainException("User already exists");
            }
            var cacheKey = $"user_{request.Email}";
            var userCache = await _cacheService.GetAsync<UserCache>(cacheKey, cancellationToken);
            if (userCache == null)
            {
                throw new DomainException("User not found or already verified");
            }

            if (IsOtpValid(request.Otp, userCache.Otp))
            {
                var newUser = User.Register(
                    userCache.Email,
                    userCache.PasswordHash,
                    userCache.Name,
                    userCache.Telephone
                );

                newUser.Activate();
                await _userRepository.CreateAsync(newUser, cancellationToken);
                await _cacheService.RemoveAsync(cacheKey);

                return "OTP verified successfully";
            }

            var newFailedAttempts = userCache.FailedAttempts + 1;

            if (IsMaxAttemptsReached(newFailedAttempts))
            {
                await _cacheService.RemoveAsync(request.Email);
                throw new DomainException($"Otp has been entered incorrectly {MaxFailedAttempts} times. Please request a new code.");
            }

            var updatedUserCache = userCache with { FailedAttempts = newFailedAttempts };
            await _cacheService.SetAsync(request.Email, updatedUserCache);

            int remaining = GetRemainingAttempts(newFailedAttempts);
            throw new DomainException($"Otp not correct. {remaining} tries remaining.");
        }
        private static bool IsOtpValid(string providedOtp, string cachedOtp)
        {
            return providedOtp == cachedOtp;
        }

        private static bool IsMaxAttemptsReached(int failedAttempts)
        {
            return failedAttempts >= MaxFailedAttempts;
        }

        private static int GetRemainingAttempts(int failedAttempts)
        {
            return MaxFailedAttempts - failedAttempts;
        }

   
    }
}