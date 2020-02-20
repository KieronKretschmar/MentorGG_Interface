using Entities.Models;
using MentorInterface.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace MentorInterfaceTests
{
    [TestClass]
    public class AttributeTests
    {
        [DataRow("1,2,3", true)]
        [DataRow("1", true)]
        [DataRow("1;2;3", false)]
        [DataRow("1-2-3", false)]
        [DataRow("a", false)]
        [DataTestMethod]
        public void ValidateMatchIdsTest_BadFormat_ResultsInBadRequestResult(string matchIdString, bool expectedToBeValid)
        {
            //// Arrange
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                filters: new List<IFilterMetadata>(), // for majority of scenarios you need not worry about populating this parameter
                actionArguments: new Dictionary<string, object>
                {
                    { ValidateMatchIds.MatchIdsArgument, matchIdString }
                }, // if the filter uses this data, add some data to this dictionary
                controller: null); // since the filter being tested here does not use the data from this parameter, just provide null
            var validationFilter = new ValidateMatchIds();

            // Act
            // Add an erorr into model state on purpose to make it invalid
            //actionContext.ModelState.AddModelError("Age", "Age cannot be below 18 years.");
            validationFilter.OnActionExecuting(actionExecutingContext);

            // Assert
            if (expectedToBeValid)
            {
                Assert.IsNull(actionExecutingContext.Result);
            }
            else
            {
                Assert.AreEqual(typeof(BadRequestResult), actionExecutingContext.Result.GetType());
                //Assert.(400, actionExecutingContext.Result.Response.StatusCode);
            }
        }
    }
}
