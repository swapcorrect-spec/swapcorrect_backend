using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SwapShop.Api.ResponsHandler
{
    public static class MediatorResponseHelper
    {
        public static async Task<IActionResult> Handle<TResponse>(
            IMediator mediator, IRequest<TResponse> request, ControllerBase controller)
            where TResponse : class
        {
            var result = await mediator.Send(request);

            var statusCodeProp = typeof(TResponse).GetProperty("StatusCode");
            if (statusCodeProp == null)
                return controller.BadRequest("Missing StatusCode property on response.");

            var statusCode = (int)statusCodeProp.GetValue(result);

            return statusCode switch
            {
                200 or 201 => controller.Ok(result),
                404 => controller.NotFound(result),
                _ => controller.BadRequest(result)
            };
        }
    }
}

