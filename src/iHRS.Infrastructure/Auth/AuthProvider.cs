using iHRS.Application.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace iHRS.Infrastructure.Auth
{
    internal sealed class AuthProvider : IAuthProvider
    {
        private readonly HttpContext _httpContext;

        public AuthProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public Guid UserId => Guid.TryParse(_httpContext.User?.Identity?.Name, out var userId) ? userId : Guid.Empty;

        public Guid CustomerId =>
            _httpContext.User.IsInRole("customer")
                ? Guid.TryParse(_httpContext.User?.Identity?.Name, out var userId) ? userId : Guid.Empty
                : Guid.Empty;

        public Guid TenantId => Guid.TryParse(_httpContext?.User?.Claims.SingleOrDefault(c => c.Type.ToLower() == "tenantid")?.Value, out var tenantId) ? tenantId : Guid.Empty; 
    }
}
