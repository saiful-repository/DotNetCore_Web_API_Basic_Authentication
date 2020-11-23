using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Web_API_Basic_Authentication.Model;

namespace Web_API_Basic_Authentication.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BasicAuthentication
    {
        private readonly RequestDelegate _next;        

        public BasicAuthentication(RequestDelegate next)
        {
            _next = next;            
        }

        public async Task Invoke(HttpContext httpContext)
        {

            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserPassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                string userPassword = encoding.GetString(Convert.FromBase64String(encodedUserPassword));

                int index = userPassword.IndexOf(":");
                string username = userPassword.Substring(0, index);
                string password = userPassword.Substring(index + 1);

                var userManager = (UserManager<ApplicationUser>) httpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>));
                var user = await userManager.FindByNameAsync(username);

                //bool IsValid = await _authenticatecs.IsAuthenticate(username, password);
                if (user != null && await userManager.CheckPasswordAsync(user, password))
                {
                    await _next.Invoke(httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BasicAuthenticationExtensions
    {
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthentication>();
        }
    }
}
