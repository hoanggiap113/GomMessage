using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Common;
using GomMessage.Domain.Entities;
using GomMessage.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Tenants.Commands
{
    public sealed class InviteMemberCommandHandler : IRequestHandler<InviteMemberCommand>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMailService _emailService;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;   
        private readonly ICurrentUserService _currentUserService;

        public InviteMemberCommandHandler(
            ITenantRepository tenantRepository, 
            IMailService emailService, 
            IInvitationRepository invitationRepository, 
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            ICurrentUserService currentUserService
            )
        {
            _tenantRepository = tenantRepository;
            _emailService = emailService;
            _invitationRepository = invitationRepository;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(
            InviteMemberCommand request, 
            CancellationToken cancellationToken)
        {
            //Validate if invited user is already a member of the tenant
            var existTenant = await _tenantRepository
                .GetTenantById(request.TenantId.ToString(), cancellationToken);
            if (existTenant == null) { 
                throw new BadRequestException(ErrorCode.TenantNotFound, "Tenant not found");
            }

            bool isAlreadyMember = await _tenantRepository
                .IsUserExistInTenantAsync(request.TenantId, request.Email, cancellationToken);
            if(isAlreadyMember)
            {
                throw new BadRequestException(ErrorCode.AlreadyMember, "User is already a member of the tenant");
            }
            bool hasPendingInvite = await _invitationRepository.HasPendingInvitationAsync(
                request.TenantId,
                request.Email,
                cancellationToken);

            if (hasPendingInvite)
            {
                throw new BadRequestException(ErrorCode.InvitationPendingExists, "An active invitation has already been sent to this email");
            }
            (string rawToken, string tokenHash) = _tokenService.GenerateInvitationToken();


            Invitation invitation = Invitation.Create(
                request.TenantId,
                request.Email,
                request.Role,
                tokenHash,
                Guid.Parse(_currentUserService.UserId),
                DateTimeOffset.UtcNow.AddDays(1)
                );

            _invitationRepository.AddInvitation(invitation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _emailService.SendInvitation(
                request.Email,
                existTenant.Name,
                "Chủ tenant",
                $"https://yourapp.com/invitation?token={rawToken}"
                );
            return Unit.Value;
        }

    }
}
