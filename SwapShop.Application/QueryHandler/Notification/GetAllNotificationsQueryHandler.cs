using MediatR;
using SwapShop.Application.Queries.Notification;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Notification;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Notification
{
    public class GetAllNotificationsQueryHandler
        : IRequestHandler<GetAllNotificationsQuery, ResponseDto<PaginatedResult<NotificationDto>>>
    {
        private readonly INotificationService _notificationService;

        public GetAllNotificationsQueryHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task<ResponseDto<PaginatedResult<NotificationDto>>> Handle(
            GetAllNotificationsQuery request, CancellationToken cancellationToken)
            => _notificationService.GetAllNotificationsAsync(
                request.UserId, request.Type, request.PageNumber, request.PageSize);
    }
}
