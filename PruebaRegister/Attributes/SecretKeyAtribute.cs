using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaRegister.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class SecretKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string SECRETKEY = "SecretKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(SECRETKEY, out var extractedSecretKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Secret Key was not provided"
                };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var SecretKey = appSettings.GetValue<string>(SECRETKEY);

            if (!SecretKey.Equals(extractedSecretKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Secret Key is not valid"
                };
                return;
            }

            await next();
        }
    }
}
