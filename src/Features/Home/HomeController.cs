using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Preenactos.Infraestructure;

namespace Preenactos.Home
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public string Get() => "Hello Preenactos";

        [HttpGet]
        [JWTAuth]
        [Route("Auth")]
        public string Auth() => "You are authorized";
    }
}
