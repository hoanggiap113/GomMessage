using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Common;
using GomMessage.Domain.Entities;
using MediatR;
using System.Data;


namespace GomMessage.Application.Tenants.Commands
{
    
   public sealed class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand>
    {
    
        private readonly ITenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateTenantCommandHandler(
            ITenantRepository tenantRepository, 
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedAccessException("Unauthorized access.");
            }

            var tenant = await _tenantRepository.GetTenantByNameAsync(request.Name, cancellationToken);
            if(tenant != null)
            {
                throw new DuplicateNameException(ErrorCode.TenantNameExists.ToString());
            }
            Tenant newTenant = Tenant.Create(
                    request.Name,
                    request.Slug,
                    Guid.Parse(currentUserId),
                    request.Settings
            );
            _tenantRepository.CreateTenant(newTenant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
