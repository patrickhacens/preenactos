using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Preenactos.Infraestructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Preenactos.Features.Authentication.Login
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private IMediator mediator;

        public LoginController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<Login.Result> Login([FromBody] Login.Command value)
        {
            var result = await mediator.Send(value);

            return result;
        }
    }
}
