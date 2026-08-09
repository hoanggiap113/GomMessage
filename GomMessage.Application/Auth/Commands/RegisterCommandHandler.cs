using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Entities;
using GomMessage.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Auth.Commands
{
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IMailService _emailService;

        public RegisterCommandHandler(IUserRepository userRepository,
            ICacheService cacheService, 
            IHashPasswordService hashPasswordService, 
            IMailService emailService)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
            _hashPasswordService = hashPasswordService;
            _emailService = emailService;
        }
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
            if (existingUser)
            {
                throw new DomainException("User with this email already exists.");
            }
            var cacheKey = $"user_{request.Email}";

            var cacheUser = await _cacheService.GetAsync<User>(cacheKey, cancellationToken);
            if (cacheUser != null)
            {
                throw new DomainException("User with this email is already in the process of registration.");
            }

            var user = User.Register(
                request.Email, 
                _hashPasswordService.HashPassword(request.Password), 
                request.Name, 
                request.Telephone);

            if (cacheUser != null)
            {
                throw new DomainException("User with this email is already in the process of registration.");
            }

            string otp = GenerateOtp();
            var userCache = new UserCache(
                user.Name,
                user.Email,
                user.PasswordHash,
                otp,
                user.Telephone);
            await _cacheService.SetAsync(cacheKey, userCache,
                 TimeSpan.FromHours(1),cancellationToken);

            await _emailService.SendOtpCode(user.Email, user.Email, otp);
            
            return "success register!";
    
        }
        private string GenerateOtp(int length = 6)
        {
            int max = (int)Math.Pow(10, length);
            int randomNumber = RandomNumberGenerator.GetInt32(0, max);

            return randomNumber.ToString($"D{length}");
        }

    }
}
