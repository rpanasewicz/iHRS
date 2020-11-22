using System;

namespace iHRS.Application.Auth
{
    public interface IAuthProvider
    {
        Guid UserId { get; }
        Guid CustomerId { get; }
        Guid TenantId { get; }
    }
}
