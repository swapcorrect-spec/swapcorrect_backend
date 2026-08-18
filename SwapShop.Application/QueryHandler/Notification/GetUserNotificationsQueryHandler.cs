using MediatR;
using SwapShop.Application.Queries.Notification;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Notification;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Notification
{
    public class GetUserNotificationsQueryHandler
        : IRequestHandler<GetUserNotificationsQuery, ResponseDto<PaginatedResult<NotificationDto>>>
    {
        private readonly INotificationService _notificationService;

        public GetUserNotificationsQueryHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task<ResponseDto<PaginatedResult<NotificationDto>>> Handle(
            GetUserNotificationsQuery request, CancellationToken cancellationToken)
            => _notificationService.GetUserNotificationsAsync(
                request.UserId, request.UnreadOnly, request.PageNumber, request.PageSize);
    }
}
