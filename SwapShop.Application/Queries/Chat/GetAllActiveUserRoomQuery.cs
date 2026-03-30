using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Chat;

namespace SwapShop.Application.Queries.Chat
{
    public class GetAllActiveUserRoomQuery : IRequest<ResponseDto<List<GetAllActiveChatResp>>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
