using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
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
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterUserResponse>
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
        public async Task<RegisterUserResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.ExistsByEmailAsync(request.Email);
            if (existingUser)
            {
                throw new DomainException("User with this email already exists.");
            }


            var user = User.Register(
                request.Email, 
                _hashPasswordService.HashPassword(request.Password), 
                request.Name, 
                request.Telephone);
            string otp = GenerateOtp();
            var cacheKey = $"user_{user.Id}";
            await _cacheService.SetAsync(cacheKey, new { user.Id, user.Email, user.Name, user.PasswordHash, user.Telephone,otp },
                 TimeSpan.FromHours(1),cancellationToken);

            await _emailService.SendOtpCode(user.Email, user.Email, otp);
            
            return new RegisterUserResponse("success register!");
    
        }
        private string GenerateOtp(int length = 6)
        {
            int max = (int)Math.Pow(10, length);
            int randomNumber = RandomNumberGenerator.GetInt32(0, max);

            return randomNumber.ToString($"D{length}");
        }

    }
}
