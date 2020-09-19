using System;
using iHRS.Application.Auth;
using Microsoft.AspNetCore.Http;

namespace iHRS.Infrastructure.Auth
{
    internal sealed class AuthProvider : IAuthProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public AuthProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public Guid UserId => Guid.TryParse(_httpContext.User?.Identity?.Name, out var userId) ? userId : Guid.Empty;
    }
}
