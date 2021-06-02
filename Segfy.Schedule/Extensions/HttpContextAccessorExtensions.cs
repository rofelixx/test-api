using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Segfy.Schedule.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        private const string SEGFY = "segfy";
        private const string SUBSCRIPTIONID = "subscription_id";
        private const string USERID = "user_id";
        private const string BROKERID = "broker_id";


        public static string GetSegfy(this IHttpContextAccessor httpContext)
        {
            return httpContext.HttpContext.User.FindFirst(SEGFY)?.Value;
        }

        public static string GetUserId(this IHttpContextAccessor httpContext)
        {
            return httpContext.HttpContext.User.FindFirst(USERID)?.Value;
        }

        public static string GetSubscriptionId(this IHttpContextAccessor httpContext)
        {
            var item = GetDynamic(httpContext, SUBSCRIPTIONID);
            return item.ToString();
        }

        public static string GetBrokerId(this IHttpContextAccessor httpContext)
        {
            var item = GetDynamic(httpContext, BROKERID);
            return item.ToString();
        }

        private static dynamic GetDynamic(IHttpContextAccessor httpContext, string key)
        {
            string json = GetSegfy(httpContext);
            if (string.IsNullOrWhiteSpace(json))
            {
                return string.Empty;
            }

            var values = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(json);
            return values.FirstOrDefault(x => x.Key == key).Value;
        }
    }
}