using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Segfy.Schedule.Filters;
using Xunit;

namespace Segfy.Schedule.Tests.Filters
{
    public class ValidationFilterTests
    {
        [Fact]
        public void OnActionExecuting_WillFailWhen_ModelIsNotValid()
        {
            //Given 
            var filter = new ValidationFilter();
            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>()
            );
            actionContext.ModelState.AddModelError("error", "error");

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

            //When
            filter.OnActionExecuting(actionExecutingContext);

            //Then
            actionExecutingContext.Result.Should().BeAssignableTo<UnprocessableEntityObjectResult>("model state is not valid");
        }

        [Fact]
        public void OnActionExecuting_WillSucceedWhen_ModelIsValid()
        {
            //Given 
            var filter = new ValidationFilter();
            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>()
            );

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );
            actionExecutingContext.Result = new OkResult();

            //When
            filter.OnActionExecuting(actionExecutingContext);

            //Then
            actionExecutingContext.Result.Should().BeAssignableTo<OkResult>("model state is valid");
        }
    }
}