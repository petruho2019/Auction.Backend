using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Attributes.Class.Filters
{
    public class RefreshAndAccessCheckFilter : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if ((!context.HttpContext.Request.Path.Value!.Equals("/api/auth/login", StringComparison.OrdinalIgnoreCase)) &&
                (!context.HttpContext.Request.Path.Value!.Equals("/api/auth/register", StringComparison.OrdinalIgnoreCase)))
            {
                

                return;
            }

            await next();
            Console.WriteLine("After invoke next()");
        }
    }
}
