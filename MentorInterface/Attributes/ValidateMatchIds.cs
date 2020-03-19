using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Attributes
{
    /// <summary>
    /// An ActionFilter that validates the string parameter matchIds to be made up of numbers seperated by the defined Seperator.
    /// </summary>
    public class ValidateMatchIds : ActionFilterAttribute
    {
        /// <summary>
        /// The seperator used for dividing different matchids.
        /// </summary>
        private const char Seperator = ',';

        /// <summary>
        /// The name of the matchIds argument.
        /// </summary>
        public static readonly string MatchIdsArgument = "matchIds";

        /// <summary>
        /// OnActionExecuting override.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments[MatchIdsArgument] == null)
            {
                context.ActionArguments[MatchIdsArgument] = "";
            }
            var matchIds = context.ActionArguments[MatchIdsArgument].ToString();
            var isValid = matchIds.Split(Seperator).All(x => x.All(c => Char.IsDigit(c)));

            if (!isValid)
            {
                context.Result = new BadRequestResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
