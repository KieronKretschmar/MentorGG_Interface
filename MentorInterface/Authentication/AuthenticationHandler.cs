using AspNet.Security.OpenId;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Authentication
{
    /// <summary>
    /// Handle authentication events.
    /// </summary>
    public class AuthenticationHandler
    {

        /// <summary>
        /// Handle a OnValidated event.
        /// </summary>
        /// <param name="context">CookieValidatePrincipalContext</param>
        /// <returns></returns>
        public static Task OnValidated(CookieValidatePrincipalContext context)
        {
            System.Diagnostics.Debug.WriteLine("A User has been validated");
            System.Diagnostics.Debug.WriteLine(context.Principal.Identity.Name);
            return Task.CompletedTask;
        }
    }
}
