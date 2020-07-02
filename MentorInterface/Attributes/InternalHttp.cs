using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MentorInterface.Attributes
{
    /// <summary>
    /// An ActionFilter that confirms the HTTP Call is coming from within the cluster.
    /// </summary>
    public class InternalHttp : ActionFilterAttribute
    {

        /// <summary>
        /// If the host is api.mentor.gg
        /// Which the nginx-ingress-controller appends to each call
        /// Forbid it to the shadow realm! (╯°□°）╯︵ ┻━┻
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.SingleOrDefault(x => x.Key == "Host").Value == "api.mentor.gg")
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}