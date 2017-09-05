using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Preenactos.Infraestructure
{
    public class ValidationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //nothing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new ContentResult();
                string content = JsonConvert.SerializeObject(context.ModelState,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                result.Content = content;
                result.ContentType = "application/json";

                context.HttpContext.Response.StatusCode = 400;
                context.Result = result;
            }
        }
    }
}