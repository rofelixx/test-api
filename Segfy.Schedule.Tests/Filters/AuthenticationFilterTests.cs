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
    public class AuthenticationFilterTests
    {
        [Fact]
        public void OnActionExecuting_WillFailWhen_NoContextAreSet()
        {
            //Given
            var mockContext = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockContext.Setup(_ => _.HttpContext).Returns(context);

            var filter = new AuthenticationFilter(mockContext.Object);
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

            //When
            filter.OnActionExecuting(actionExecutingContext);

            //Then
            actionExecutingContext.Result.Should().BeAssignableTo<UnauthorizedObjectResult>("no cookie context was given");
        }

        [Fact]
        public void OnActionExecuting_WillFailWhen_EmptySubscriptionId()
        {
            //Given
            var mockContext = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();

            var claims = new List<Claim>()
            {
                new Claim("segfy", "{}"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            context.User = claimsPrincipal;
            mockContext.Setup(_ => _.HttpContext).Returns(context);

            var filter = new AuthenticationFilter(mockContext.Object);
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

            //When
            filter.OnActionExecuting(actionExecutingContext);

            //Then
            actionExecutingContext.Result.Should().BeAssignableTo<UnauthorizedObjectResult>("a valid subscriptionid is necessary to authenticate");
        }

        [Fact]
        public void OnActionExecuting_WillFailWhen_EmptyUserId()
        {
            //Given
            var mockContext = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();

            var claims = new List<Claim>()
            {
                new Claim("segfy", "{\"subscription_id\": \"bae26dd5-3844-4da6-9669-d509d3f92b79\", \"broker_id\": \"bae26dd5-3844-4da6-9669-d509d3f92b79\"}"),
                new Claim("user_id", ""),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            context.User = claimsPrincipal;
            mockContext.Setup(_ => _.HttpContext).Returns(context);

            var filter = new AuthenticationFilter(mockContext.Object);
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

            //When
            filter.OnActionExecuting(actionExecutingContext);

            //Then
            actionExecutingContext.Result.Should().BeAssignableTo<UnauthorizedObjectResult>("a valid userid is necessary to authenticate");
        }

        [Fact]
        public void OnActionExecuting_WillFailWhen_EmptyBrokerId()
        {
            //Given
            var mockContext = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();

            var claims = new List<Claim>()
            {
                new Claim("segfy", "{\"subscription_id\": \"bae26dd5-3844-4da6-9669-d509d3f92b79\", \"broker_id\": \"\"}"),
                new Claim("user_id", "2045b343-4982-403b-bc1b-24d84e63439c"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            context.User = claimsPrincipal;
            mockContext.Setup(_ => _.HttpContext).Returns(context);

            var filter = new AuthenticationFilter(mockContext.Object);
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

            //When
            filter.OnActionExecuting(actionExecutingContext);

            //Then
            actionExecutingContext.Result.Should().BeAssignableTo<UnauthorizedObjectResult>("a valid brokerid is necessary to authenticate");
        }

        [Fact]
        public void OnActionExecuting_WillFailWhen_RouteSubscriptionIdDiffersFromCookie()
        {
            //Given
            var mockContext = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();

            var claims = new List<Claim>()
            {
                new Claim("segfy", "{\"subscription_id\": \"bae26dd5-3844-4da6-9669-d509d3f92b79\", \"broker_id\": \"685cfd61-08ba-45df-846d-94a278322b30\"}"),
                new Claim("user_id", "2045b343-4982-403b-bc1b-24d84e63439c"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            context.User = claimsPrincipal;
            mockContext.Setup(_ => _.HttpContext).Returns(context);

            var routeData = new RouteData();
            routeData.Values.Add("subscriptionId", "2045b343-4982-403b-bc1b-24d84e63439c");

            var filter = new AuthenticationFilter(mockContext.Object);
            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                routeData,
                Mock.Of<ActionDescriptor>()
            );

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

            //When
            filter.OnActionExecuting(actionExecutingContext);

            //Then
            actionExecutingContext.Result.Should().BeAssignableTo<UnauthorizedObjectResult>("the same subscriptionid should be given on both the route and the cookie jwt");
        }

        [Fact]
        public void OnActionExecuting_WillFailWhen_RouteSubscriptionIdIsNotSet()
        {
            //Given
            var mockContext = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();

            var claims = new List<Claim>()
            {
                new Claim("segfy", "{\"subscription_id\": \"bae26dd5-3844-4da6-9669-d509d3f92b79\", \"broker_id\": \"685cfd61-08ba-45df-846d-94a278322b30\"}"),
                new Claim("user_id", "2045b343-4982-403b-bc1b-24d84e63439c"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            context.User = claimsPrincipal;
            mockContext.Setup(_ => _.HttpContext).Returns(context);

            var routeData = new RouteData();
            var filter = new AuthenticationFilter(mockContext.Object);
            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                routeData,
                Mock.Of<ActionDescriptor>()
            );

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

            //When
            filter.OnActionExecuting(actionExecutingContext);

            //Then
            actionExecutingContext.Result.Should().BeAssignableTo<UnauthorizedObjectResult>("a valid subscriptionid from route is necessary to authenticate");
        }

        [Fact]
        public void OnActionExecuting_WillSucceedWhen_AllTokemValidationsPass()
        {
            //Given
            var mockContext = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();

            var claims = new List<Claim>()
            {
                new Claim("segfy", "{\"subscription_id\": \"bae26dd5-3844-4da6-9669-d509d3f92b79\", \"broker_id\": \"685cfd61-08ba-45df-846d-94a278322b30\"}"),
                new Claim("user_id", "2045b343-4982-403b-bc1b-24d84e63439c"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            context.User = claimsPrincipal;
            mockContext.Setup(_ => _.HttpContext).Returns(context);

            var routeData = new RouteData();
            routeData.Values.Add("subscriptionId", "bae26dd5-3844-4da6-9669-d509d3f92b79");

            var filter = new AuthenticationFilter(mockContext.Object);
            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                routeData,
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
            actionExecutingContext.Result.Should().NotBeAssignableTo<UnauthorizedObjectResult>("all validations are passing");
            actionExecutingContext.Result.Should().BeAssignableTo<OkResult>("the result type should remain the same when auth is valid");
        }
    }
}