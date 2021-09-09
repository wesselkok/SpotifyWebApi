using Microsoft.AspNetCore.Mvc.Filters;
using SpotifyApp.Core.Config;
using System;

namespace SpotifyApp.Api.Attributes
{
    public class TokenRequiredAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Check token
            var configuration = SpotifyConfiguration.Instance;
            if (string.IsNullOrWhiteSpace(configuration.Token)) throw new Exception("Invalid token");
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}
