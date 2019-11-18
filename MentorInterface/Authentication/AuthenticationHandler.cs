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
        /// Handle a successful authentication challenge.
        /// </summary>
        /// <param name="context">OpenIdAuthenticatedContext</param>
        /// <returns></returns>
        public static Task HandleSuccess(OpenIdAuthenticatedContext context)
        {
            System.Diagnostics.Debug.WriteLine("AUTH");
            System.Diagnostics.Debug.WriteLine(context.Identity.Name);

            return Task.CompletedTask;
        }

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
