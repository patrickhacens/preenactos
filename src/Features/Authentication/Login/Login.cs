using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Preenactos.Infraestructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Preenactos.Features.Authentication.Login
{
    public class Login
    {
        public class Command : IRequest<Result>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Username).NotNull().NotEmpty();
                RuleFor(c => c.Password).NotNull().NotEmpty();
            }
        }

        public class Result
        {
            public string Token { get; set; }

            public DateTime Expiration { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private readonly Db db;
            private readonly IConfiguration configuration;

            public Handler(Db db, IStartup startup, IConfiguration configuration)
            {
                this.db = db;
                if (startup is Startup s)
                    this.configuration = s.Configuration;
                else this.configuration = configuration;
            }

            public async Task<Result> Handle(Command message)
            {
                Domain.User user = await db.Users.SingleOrDefaultAsync(usern => usern.Email == message.Username);

                if (user == null || !user.IsPasswordEqualsTo(message.Password)) throw new HttpException(401);

                //if (user.DeletedAt != null) throw new UnauthorizedException();

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(configuration["Tokens:Issuer"],
                                                  configuration["Tokens:Audience"],
                                                  claims,
                                                  expires: DateTime.Now.AddYears(1),
                                                  signingCredentials: creds);

                return new Result
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };
            }
        }
    }
}
