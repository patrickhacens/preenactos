using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Preenactos.Infraestructure
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (HttpException e)
            {
                context.Response.StatusCode = e.StatusCode;
                if (e.Body != null)
                {
                }
            }
        }
    }
}