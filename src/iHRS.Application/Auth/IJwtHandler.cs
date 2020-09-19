using System.Collections.Generic;

namespace iHRS.Application.Auth
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(string userId, string role, IDictionary<string, IEnumerable<string>> claims = null);
        JsonWebTokenPayload GetTokenPayload(string accessToken);
    }
}