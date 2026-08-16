using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Domain.Constants
{
    public static class CacheKey
    {
        private const string Prefix = "app"; 

        // Master department cache
        public const string AllTenants = "tenants:all";
        public static string TenantById(string tenantId) => $"tenants:{tenantId}";

        // User-specific departments
        public static string UserTenants(string userId) => $"users:{userId}:tenants";
        public static string UserTenantIds(string userId) => $"users:{userId}:tenant-ids";

        // Multi-tenant nếu có
        public static string TenantUserTenants(string tenantId, string userId)
            => $"tenants:{tenantId}:users:{userId}:tenants";
    }
}
