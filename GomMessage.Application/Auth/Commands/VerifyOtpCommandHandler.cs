using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Entities;
using GomMessage.Domain.Exceptions;
using MediatR;

namespace GomMessage.Application.Auth.Commands
{
    public sealed class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, string>
    {
        private const int MaxFailedAttempts = 4;

        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyOtpCommandHandler(
            IUserRepository userRepository,
            ICacheService cacheService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
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

                _userRepository.AddUser(newUser);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _cacheService.RemoveAsync(cacheKey, cancellationToken);

                return "OTP verified successfully";
            }

            var newFailedAttempts = userCache.FailedAttempts + 1;

            if (IsMaxAttemptsReached(newFailedAttempts))
            {
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
                throw new DomainException($"Otp has been entered incorrectly {MaxFailedAttempts} times. Please request a new code.");
            }

            var updatedUserCache = userCache with { FailedAttempts = newFailedAttempts };

            await _cacheService.SetAsync(cacheKey, updatedUserCache,TimeSpan.FromMinutes(60), cancellationToken);

            int remaining = GetRemainingAttempts(newFailedAttempts);
            throw new DomainException($"Otp not correct. {remaining} tries remaining.");
        }

        private static bool IsOtpValid(string providedOtp, string cachedOtp) => providedOtp == cachedOtp;
        private static bool IsMaxAttemptsReached(int failedAttempts) => failedAttempts >= MaxFailedAttempts;
        private static int GetRemainingAttempts(int failedAttempts) => MaxFailedAttempts - failedAttempts;
    }
}