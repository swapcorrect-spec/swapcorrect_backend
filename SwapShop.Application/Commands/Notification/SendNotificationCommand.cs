using MediatR;
using SwapShop.Domain.Dtos.Request.Notification;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Notification
{
    public class SendNotificationCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "General";
        public string? ReferenceId { get; set; }
    }
}
