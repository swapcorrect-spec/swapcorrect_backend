using MediatR;
using SwapShop.Application.Commands.Notification;
using SwapShop.Domain.Dtos.Request.Notification;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Notification
{
    public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, ResponseDto<string>>
    {
        private readonly INotificationService _notificationService;

        public SendNotificationCommandHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<ResponseDto<string>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            await _notificationService.SendNotificationAsync(new SendNotificationRequest
            {
                UserId = request.UserId,
                Title = request.Title,
                Message = request.Message,
                Type = request.Type,
                ReferenceId = request.ReferenceId
            });

            return new ResponseDto<string>
            {
                StatusCode = 200,
                DisplayMessage = "Success",
                Result = "Notification sent"
            };
        }
    }
}
