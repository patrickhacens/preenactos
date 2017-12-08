using Microsoft.AspNetCore.Identity;
using System;

namespace Preenactos.Domain
{
    public class Role : IdentityRole<Guid>
    {
    }

    public class RoleClaim : IdentityRoleClaim<Guid>
    {
    }

    public class UserClaim : IdentityUserClaim<int>
    {
    }

    public class User_Role : IdentityUserRole<Guid>
    {
    }
}