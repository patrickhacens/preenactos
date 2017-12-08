using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Preenactos.Infraestructure
{
    public static class DbInitializer
    {

        public static async Task Initialize(Db db, ILogger logger)
        {

            var admin = new Domain.User()
            {
                Email = "admin@admin.com",
                Name = "administrator",
                UserName = "admin"
            };
            admin.SetPassword("Asdf1234");
            db.Users.Add(admin);
            await db.SaveChangesAsync();
        }
    }
}
