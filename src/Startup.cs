using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Preenactos.Infraestructure;
using Preenactos.Domain;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Preenactos
{
    public class Startup : IStartup
    {
        public Action<DbContextOptionsBuilder> DatabaseConfigurationAction { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            DatabaseConfigurationAction = GetDatabaseConfiguration();
        }

        public IConfiguration Configuration { get; }

        IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ValidationActionFilter));
            })
                    .AddFeatureFolders()
                    .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    });

            services.AddDbContext<Db>(DatabaseConfigurationAction)
                    .AddAutoMapper(typeof(Startup));

            services.AddIdentityCore<User>(options =>
            {
                options.Password = new PasswordOptions() { RequiredLength = 8 };
                options.User = new UserOptions() { RequireUniqueEmail = true };
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<Db>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                        ValidateAudience = true,
                        ValidAudience = Configuration["Tokens:Audience"],

                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Tokens:Issuer"],

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            Mapper.AssertConfigurationIsValid();

            services.AddMediatR(typeof(Startup));

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseDatabaseErrorPage();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseMvcWithDefaultRoute();
        }

        public Action<DbContextOptionsBuilder> GetDatabaseConfiguration() => options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    }
}