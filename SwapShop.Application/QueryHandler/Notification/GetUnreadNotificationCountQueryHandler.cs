using MediatR;
using SwapShop.Application.Queries.Notification;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Notification
{
    public class GetUnreadNotificationCountQueryHandler
        : IRequestHandler<GetUnreadNotificationCountQuery, ResponseDto<int>>
    {
        private readonly INotificationService _notificationService;

        public GetUnreadNotificationCountQueryHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task<ResponseDto<int>> Handle(
            GetUnreadNotificationCountQuery request, CancellationToken cancellationToken)
            => _notificationService.GetUnreadCountAsync(request.UserId);
    }
}
