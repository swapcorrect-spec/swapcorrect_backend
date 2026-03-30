using Microsoft.AspNetCore.SignalR;
using SwapShop.Domain.Dtos.Response.Chat;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IAccountService _accountService;
        private static readonly Dictionary<string, string> _onlineUsers = new();
        public ChatHub(IChatService chatService, IAccountService accountService)
        {
            _chatService = chatService;
            _accountService = accountService;
        }

        public async Task RegisterUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                _onlineUsers[userId] = Context.ConnectionId;

                // Optionally mark user online in DB
                await _accountService.ChangeOnlineStatus(userId, true);

                // Notify everyone
                await Clients.All.SendAsync("UserOnline", userId);
            }
        }
        public async Task DisconectUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                _onlineUsers[userId] = Context.ConnectionId;

                // Optionally mark user online in DB
                await _accountService.ChangeOnlineStatus(userId, false);

                // Notify everyone
                await Clients.All.SendAsync("UserOffline", userId);
            }
        }



        public async Task JoinRoom(string roomName, string userId)
        {
            await _accountService.ChangeOnlineStatus(userId, true);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessageToRoom(string roomName, string userId, string message, string messageType)
        {
            var checkUser = await _accountService.GetUserbyId(userId);
            if (checkUser != null)
            {
                var addMessage = await _chatService.AddMessage(userId, roomName, message, messageType);
                var newMsg = new RoomMessageResp()
                {
                    message = message,
                    messageType = messageType,
                    senderImgUrl = checkUser.Result.ProfilePicture,
                    senderId = userId,
                    DateTime = addMessage.Result.Created.ToShortTimeString()
                };
                await Clients.Group(roomName).SendAsync("ReceiveMessage", userId, newMsg);
            }

        }


    }

}
