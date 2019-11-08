using AspNet.Security.OpenId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Authentication
{
    public class AuthenticationHandler
    {
        public static Task HandleSuccess(OpenIdAuthenticatedContext context)
        {
            System.Diagnostics.Debug.WriteLine("AUTH");
            System.Diagnostics.Debug.WriteLine(context.Identity.Name);

            // Required
            return Task.FromResult<object>(null);
        }
    }
}
