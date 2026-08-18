using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Notification;

namespace SwapShop.Application.Queries.Notification
{
    public class GetUserNotificationsQuery : IRequest<ResponseDto<PaginatedResult<NotificationDto>>>
    {
        public string UserId { get; set; }
        public bool? UnreadOnly { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
