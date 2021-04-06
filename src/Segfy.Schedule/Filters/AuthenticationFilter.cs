using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Segfy.Schedule.Extensions;

namespace Segfy.Schedule.Filters
{
    public class AuthenticationFilter : IActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationFilter(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var cookieSubscriptionId = _httpContextAccessor.GetSubscriptionId();
                if (!string.IsNullOrWhiteSpace(cookieSubscriptionId))
                {
                    throw new System.Exception("Subscription ID is missing from token.");
                }

                var cookieUserId = _httpContextAccessor.GetUserId();
                if (!string.IsNullOrWhiteSpace(cookieUserId))
                {
                    throw new System.Exception("User ID is missing from token.");
                }

                var cookieBrokerId = _httpContextAccessor.GetBrokerId();
                if (!string.IsNullOrWhiteSpace(cookieBrokerId))
                {
                    throw new System.Exception("Broker ID is missing from token.");
                }

                var routeSubscriptionId = _httpContextAccessor.GetRouteSubscriptionId();
                if (cookieSubscriptionId != routeSubscriptionId)
                {
                    throw new System.Exception("Cookie does not belong to this subscription.");
                }
            }
            catch (System.Exception ex)
            {
                context.Result = new UnauthorizedObjectResult($"Error while handling SSO token. {ex.Message}");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}