using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Queries.Notification
{
    public class GetUnreadNotificationCountQuery : IRequest<ResponseDto<int>>
    {
        public string UserId { get; set; }
    }
}
