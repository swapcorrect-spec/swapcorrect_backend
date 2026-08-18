using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Notification
{
    public class MarkNotificationReadCommand : IRequest<ResponseDto<string>>
    {
        public string NotificationId { get; set; }
        public string UserId { get; set; }
    }
}
