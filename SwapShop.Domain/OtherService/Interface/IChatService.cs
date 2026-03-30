using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Chat;
using SwapShop.Domain.Enitities;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IChatService
    {
        Task<ResponseDto<RoomMessages>> AddMessage(string userId, string roomName, string Message, string msgType);
        Task<ResponseDto<int>> GetUnreadMessageCount(string userId);
        Task<ResponseDto<List<GetAllActiveChatResp>>> GetAllActiveUserRoom(string userId);
        Task<ResponseDto<GetFullRoomMessageRep>> GetRoomMessage(string userId, string RoomName);
    }
}
