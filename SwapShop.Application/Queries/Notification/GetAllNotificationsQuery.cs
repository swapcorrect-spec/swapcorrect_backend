using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Notification;

namespace SwapShop.Application.Queries.Notification
{
    public class GetAllNotificationsQuery : IRequest<ResponseDto<PaginatedResult<NotificationDto>>>
    {
        public string? UserId { get; set; }
        public string? Type { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
