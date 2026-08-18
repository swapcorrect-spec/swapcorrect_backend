using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Notification
{
    public class MarkAllNotificationsReadCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
    }
}
