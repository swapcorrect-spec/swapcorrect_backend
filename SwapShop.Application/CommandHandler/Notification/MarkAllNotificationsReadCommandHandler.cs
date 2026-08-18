using MediatR;
using SwapShop.Application.Commands.Notification;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Notification
{
    public class MarkAllNotificationsReadCommandHandler : IRequestHandler<MarkAllNotificationsReadCommand, ResponseDto<string>>
    {
        private readonly INotificationService _notificationService;

        public MarkAllNotificationsReadCommandHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task<ResponseDto<string>> Handle(MarkAllNotificationsReadCommand request, CancellationToken cancellationToken)
            => _notificationService.MarkAllAsReadAsync(request.UserId);
    }
}
