using MediatR;
using SwapShop.Application.Commands.Notification;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Notification
{
    public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, ResponseDto<string>>
    {
        private readonly INotificationService _notificationService;

        public MarkNotificationReadCommandHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task<ResponseDto<string>> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
            => _notificationService.MarkAsReadAsync(request.NotificationId, request.UserId);
    }
}
