using MediatR;
using SwapShop.Application.Queries.Chat;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Chat;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Chat
{
    public class GetRoomMessageQueryHandler : IRequestHandler<GetRoomMessageQuery, ResponseDto<GetFullRoomMessageRep>>
    {
        private readonly IChatService _chatService;

        public GetRoomMessageQueryHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<ResponseDto<GetFullRoomMessageRep>> Handle(GetRoomMessageQuery request, CancellationToken cancellationToken)
        {
            return await _chatService.GetRoomMessage(request.UserId, request.RoomName);
        }
    }
}
