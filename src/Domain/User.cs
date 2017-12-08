using Microsoft.AspNetCore.Identity;
using Preenactos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Preenactos.Domain
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }

        public string PasswordSalt { get; set; }

        public string Picture { get; set; }

        public void SetPassword(string password)
        {
            using (EncryptService service = new EncryptService())
                (this.PasswordHash, this.PasswordSalt) = service.Encrypt(password);
        }
    }
}
