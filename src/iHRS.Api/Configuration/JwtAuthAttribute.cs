using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace iHRS.Api.Configuration
{
    public class JwtAuthAttribute : AuthorizeAttribute
    {
        public JwtAuthAttribute()
        {
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
