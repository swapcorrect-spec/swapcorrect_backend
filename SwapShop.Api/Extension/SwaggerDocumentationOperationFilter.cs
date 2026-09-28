using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SwapShop.Api.Extension
{
    public sealed class SwaggerDocumentationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor action)
                return;

            var actionName = ToWords(action.ActionName);
            var controllerName = ToWords(action.ControllerName);
            var route = context.ApiDescription.RelativePath ?? string.Empty;
            var httpMethod = context.ApiDescription.HttpMethod ?? "HTTP";
            var requiresAuthentication = action.MethodInfo.GetCustomAttribute<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null
                || action.ControllerTypeInfo.GetCustomAttribute<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null;

            if (string.IsNullOrWhiteSpace(operation.Summary))
                operation.Summary = actionName;
            if (string.IsNullOrWhiteSpace(operation.Description))
                operation.Description = $"{httpMethod} endpoint for {actionName.ToLowerInvariant()} in the {controllerName} controller.\n\nRoute: `{httpMethod} /{route}`"
                    + (requiresAuthentication ? "\n\nAuthentication: Bearer token required." : "\n\nAuthentication: Public endpoint.");
        }

        private static string ToWords(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "API operation";

            return Regex.Replace(value.Replace("Async", string.Empty), "(?<!^)([A-Z])", " $1").Trim();
        }
    }
}
