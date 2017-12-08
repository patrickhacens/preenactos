using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Preenactos.Infraestructure
{
    public class JWTAuthAttribute : AuthorizeAttribute
    {
        public JWTAuthAttribute()
        {
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}