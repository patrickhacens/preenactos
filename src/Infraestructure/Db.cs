using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Preenactos.Domain;

namespace Preenactos.Infraestructure
{
    public class Db : DbContext
    {
        #region Tables

        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<User> Users { get; set; }

        #endregion 

        public Db(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder m)
        {
            base.OnModelCreating(m);

            m.Entity<User>().ToTable(nameof(User));
            m.Entity<Role>().ToTable(nameof(Role));
            m.Entity<RoleClaim>().ToTable(nameof(RoleClaim));
            m.Entity<UserLogin>().ToTable(nameof(UserLogin));
            m.Entity<UserClaim>().ToTable(nameof(UserClaim));
            m.Entity<User_Role>().ToTable(nameof(User_Role));

            m.Entity<UserLogin>().HasKey(p => new { p.LoginProvider, p.ProviderKey }).ForSqlServerIsClustered(true);

            m.Entity<User_Role>().HasKey(r => new { r.UserId, r.RoleId }).ForSqlServerIsClustered(true);
        }
    }
}