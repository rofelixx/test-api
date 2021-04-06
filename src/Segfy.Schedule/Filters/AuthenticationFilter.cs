using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Segfy.Schedule.Extensions;

namespace Segfy.Schedule.Filters
{
    public class AuthenticationFilter : IActionFilter
    {
        private const string ROUTESUBSCRIPTIONID = "subscriptionId";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationFilter(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string cookieSubscriptionId = ValidateSubscriptionId();
                ValidateUserId();
                ValidateBrokerId();
                ValidateSubscriptionIdFromRoute(context, cookieSubscriptionId);
            }
            catch (System.Exception ex)
            {
                context.Result = new UnauthorizedObjectResult($"Error while handling SSO token. {ex.Message}");
            }
        }

        private static void ValidateSubscriptionIdFromRoute(ActionExecutingContext context, string cookieSubscriptionId)
        {
            var routeSubscriptionId = GetRouteSubscriptionId(context.RouteData);
            if (cookieSubscriptionId != routeSubscriptionId)
            {
                throw new System.Exception("Cookie does not belong to this subscription.");
            }
        }

        private void ValidateBrokerId()
        {
            var cookieBrokerId = _httpContextAccessor.GetBrokerId();
            if (string.IsNullOrWhiteSpace(cookieBrokerId))
            {
                throw new System.Exception("Broker ID is missing from token.");
            }
        }

        private void ValidateUserId()
        {
            var cookieUserId = _httpContextAccessor.GetUserId();
            if (string.IsNullOrWhiteSpace(cookieUserId))
            {
                throw new System.Exception("User ID is missing from token.");
            }
        }

        private string ValidateSubscriptionId()
        {
            var cookieSubscriptionId = _httpContextAccessor.GetSubscriptionId();
            if (string.IsNullOrWhiteSpace(cookieSubscriptionId))
            {
                throw new System.Exception("Subscription ID is missing from token.");
            }

            return cookieSubscriptionId;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static string GetRouteSubscriptionId(RouteData routeData)
        {
            if (routeData.Values.TryGetValue(ROUTESUBSCRIPTIONID, out object routeValue))
            {
                return routeValue.ToString();
            }
            return "";
        }
    }
}