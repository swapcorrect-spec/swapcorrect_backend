using Austistic.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Chat;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class ChatService : IChatService
    {
        private readonly ISwapShopGenericRepo<SwappingProceeding> _swappingProceedingRepo;
        private readonly ISwapShopGenericRepo<RoomMessages> _roomMessagesRepo;
        private readonly ISwapShopGenericRepo<Room> _roomRepo;
        private readonly ISwapShopGenericRepo<ReadMassageCount> _readMassageCountRepo;
        private readonly ISwapShopGenericRepo<UserRoom> _userRoomRepo;
        private readonly ILogger<ChatService> _logger;
        public ChatService(ISwapShopGenericRepo<UserRoom> userRoomRepo, ISwapShopGenericRepo<ReadMassageCount> readMassageCountRepo, ISwapShopGenericRepo<Room> roomRepo, ISwapShopGenericRepo<RoomMessages> roomMessagesRepo, ILogger<ChatService> logger, ISwapShopGenericRepo<SwappingProceeding> swappingProceedingRepo)
        {
            _userRoomRepo = userRoomRepo;
            _readMassageCountRepo = readMassageCountRepo;
            _roomRepo = roomRepo;
            _roomMessagesRepo = roomMessagesRepo;
            _logger = logger;
            _swappingProceedingRepo = swappingProceedingRepo;
        }

        public async Task<ResponseDto<RoomMessages>> AddMessage(string userId, string roomName, string Message, string msgType)
        {
            var response = new ResponseDto<RoomMessages>();
            try
            {
                var room = await _roomRepo.GetQueryable().FirstOrDefaultAsync(u => u.RoomName == roomName);
                if (room == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid room name" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                ;
                var msgRequest = new RoomMessages
                {
                    SentById = userId,
                    MessageType = msgType,
                    Message = Message,
                    RoomId = room.Id
                };

                await _roomMessagesRepo.Add(msgRequest);
                await _roomMessagesRepo.SaveChanges();
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = msgRequest;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in saving message successfully" };
                response.StatusCode = 501;
                response.DisplayMessage = "Error";
                return response;
            }

        }

        public async Task<ResponseDto<int>> GetUnreadMessageCount(string userId)
        {
            var response = new ResponseDto<int>();
            try
            {
                // Get all rooms that user belongs to
                var userRoomIds = await _roomRepo.GetQueryable()
                    .Where(r => r.UserRoom.UserId == userId || r.UserRoom.SwapperId == userId)
                    .Select(r => r.Id)
                    .ToListAsync();

                if (!userRoomIds.Any())
                {
                    response.DisplayMessage = "Success";
                    response.Result = 0;
                    response.StatusCode = 200;
                    return response;

                }

                // Count unread messages inside those rooms
                var unreadCount = await _roomMessagesRepo.GetQueryable()
                    .Include(m => m.ReadCount)
                    .Where(m => userRoomIds.Contains(m.RoomId) &&
                                m.SentById != userId &&             // exclude self-sent
                                !m.ReadCount.Any(rc => rc.UserId == userId)) // not yet read
                    .CountAsync();
                response.DisplayMessage = "Success";
                response.Result = unreadCount;
                response.StatusCode = 200;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching unread message count for user {UserId}", userId);
                response.DisplayMessage = "Error";
                response.ErrorMessages = new List<string>() { "Error while fetching unread message count for user" };
                response.StatusCode = 400;
                return response;
            }
        }

        public async Task<ResponseDto<List<GetAllActiveChatResp>>> GetAllActiveUserRoom(string userId)
        {
            var response = new ResponseDto<List<GetAllActiveChatResp>>();
            try
            {
                var friends = await _userRoomRepo.GetQueryable()
                    .Where(f =>
                        (f.UserId == userId) ||
                        (f.SwapperId == userId))
                    .Select(f => new GetAllActiveChatResp
                    {
                        chatRooomName = f.Room.RoomName,
                        UserId = f.UserId == userId ? f.Swapper.Id : f.User.Id,
                        name = f.UserId == userId ? f.Swapper.FirstName + " " + f.Swapper.LastName : f.User.FirstName + " " + f.User.LastName,
                        message = f.Room.Messages.OrderByDescending(u => u.Created).FirstOrDefault().Message,
                        time = f.Room.Messages.OrderByDescending(u => u.Created).FirstOrDefault().Created.ToString(),
                        unreadCount = f.Room.Messages.Count(msg => msg.SentById != userId && !msg.ReadCount.Any(read => read.UserId == userId)),
                        url = f.UserId == userId ? f.Swapper.ProfilePicture : f.User.ProfilePicture,
                        userStatus = f.UserId == userId
                    ? (f.Swapper.IsOnline
                        ? "Online"
                        : (!f.Swapper.LastSeen.IsNullOrEmpty()
                            ? $"Last seen {f.Swapper.LastSeen}"
                            : "Offline"))
                    : (f.User.IsOnline
                        ? "Online"
                        : (!f.User.LastSeen.IsNullOrEmpty()
                            ? $"Last seen {f.User.LastSeen}"
                            : "Offline"))

                    }).ToListAsync();
                foreach (var user in friends)
                {

                }

                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = friends;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.ErrorMessages = new List<string> { "Error in getting user's chat partner" };
                response.StatusCode = 501;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<GetFullRoomMessageRep>> GetRoomMessage(string userId, string RoomName)
        {
            var response = new ResponseDto<GetFullRoomMessageRep>();
            try
            {
                var room = await _roomRepo.GetQueryable().Include(u=>u.UserRoom).
                    FirstOrDefaultAsync(u => u.RoomName == RoomName);
                if (room == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid room name" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }

                // Get all messages in the room
                var roomMessages = await _roomMessagesRepo.GetQueryable()
                    .Include(m => m.ReadCount)
                    .Include(u=>u.SentBy)
                    .Where(u => u.RoomId == room.Id)
                    .ToListAsync();

                // Mark messages as read for this user (if not already)
                var unreadMessages = roomMessages
                    .Where(m => !m.ReadCount.Any(rc => rc.UserId == userId)) // not read by this user yet
                    .ToList();

                if (unreadMessages.Any())
                {
                    var readEntries = unreadMessages.Select(m => new ReadMassageCount
                    {
                        MessageId = m.Id,   // from BaseEntity
                        UserId = userId,
                        ReadAt = DateTime.UtcNow
                    }).ToList();

                    await _readMassageCountRepo.AddRanges(readEntries);
                    await _readMassageCountRepo.SaveChanges();
                }
                int getImgCount = 0;
                int getVidCount = 0;
                int getFileCount = 0;
                if (roomMessages.Any())
                {
                    getImgCount = roomMessages.Where(u => u.MessageType.ToLower() == "img").Count();
                    getVidCount = roomMessages.Where(u => u.MessageType.ToLower() == "vid").Count();
                    getFileCount = roomMessages.Where(u => u.MessageType.ToLower() == "file").Count();
                }
              
                // Map to DTO
                var retrieveMessages = roomMessages.OrderByDescending(u => u.Created)
                    .Select(m => new RoomMessageResp
                    {
                        message = m.Message,
                        isMe = userId == m.SentById,
                        messageType = m.MessageType,
                        DateTime = m.Created.ToString(),
                        senderImgUrl = m.SentBy.ProfilePicture,
                        senderId = m.SentById,
                        status = m.SentById == userId || m.ReadCount.Any(rc => rc.UserId == userId) ? "Read" : "UnRead"
                    }).ToList();

                var checkProceeding = await _swappingProceedingRepo.GetQueryable()
    .Include(u => u.List)
    .FirstOrDefaultAsync(ur =>
        (
            (ur.Userid == room.UserRoom.UserId && ur.List.UserId == room.UserRoom.SwapperId) ||
            (ur.Userid == room.UserRoom.SwapperId && ur.List.UserId == room.UserRoom.UserId)
        )
        && ur.Status != SwapProceedingStatus.Swapped.ToString()
        && ur.Status != SwapProceedingStatus.Closed.ToString()
        && ur.Status != SwapProceedingStatus.AdvNegotiationSwapped.ToString()
    );

                var proce = new ChatProceedRespDto();
                if (checkProceeding != null)
                {
                    proce.Status = checkProceeding.Status;
                    proce.Userid = checkProceeding.Userid;
                    proce.Id = checkProceeding.Id;
                    proce.ListId = checkProceeding.ListId;
                }

                var result = new GetFullRoomMessageRep()
                {
                    roomMessages = retrieveMessages,
                    FileCount = getFileCount,
                    ImageCount = getImgCount,
                    VideoCount = getVidCount,
                    SwappingProceeding = proce,
                    IsSwapper = checkProceeding != null && checkProceeding.List.UserId == userId
                };
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = result;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.ErrorMessages = new List<string> { "Error in getting user's messages" };
                response.StatusCode = 501;
                response.DisplayMessage = "Error";
                return response;
            }
        }
     



    }
}
