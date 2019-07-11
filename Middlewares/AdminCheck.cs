using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatMash.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CatMash.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AdminCheck
    {
        private readonly RequestDelegate _next;
        private readonly ISecrets _secrets;

        public AdminCheck(RequestDelegate next, ISecrets secrets)
        {
            _secrets = secrets;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            StringValues password;
            httpContext.Request.Headers.TryGetValue("Authorization", out password);

            try
            {

                if (password.ElementAt(0).Equals(_secrets.AdminPassword))
                {
                    await _next(httpContext);
                }
                else
                {
                    throw new Exception("Wrong password");
                }

            }
            catch (IndexOutOfRangeException e)
            {

                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("No authorization header found on the request");
                
            }
            catch(Exception e) when (e.Message.Equals("Wrong password"))
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Wrong password");
            }
            catch (Exception e)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("You dont have permission for this request");
            }


        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AdminCheckExtensions
    {
        public static IApplicationBuilder UseAdminCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminCheck>();
        }
    }
}
