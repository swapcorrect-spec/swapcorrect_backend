using MediatR;
using SwapShop.Application.Queries.Chat;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Chat;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Chat
{
    public class GetAllActiveUserRoomQueryHandler : IRequestHandler<GetAllActiveUserRoomQuery, ResponseDto<List<GetAllActiveChatResp>>>
    {
        private readonly IChatService _chatService;

        public GetAllActiveUserRoomQueryHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<ResponseDto<List<GetAllActiveChatResp>>> Handle(GetAllActiveUserRoomQuery request, CancellationToken cancellationToken)
        {
            return await _chatService.GetAllActiveUserRoom(request.UserId);
        }
    }
}
