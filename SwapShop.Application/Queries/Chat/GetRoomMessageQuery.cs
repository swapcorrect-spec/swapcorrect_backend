using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Chat;

namespace SwapShop.Application.Queries.Chat
{
    public class GetRoomMessageQuery : IRequest<ResponseDto<GetFullRoomMessageRep>>
    {
        public string UserId { get; set; }
        public string RoomName { get; set; }
    }
}
