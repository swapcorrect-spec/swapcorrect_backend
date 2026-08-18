using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwapShop.Domain.Dtos.Request.Notification;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Notification;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly ISwapShopGenericRepo<Notification> _notificationRepo;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            ISwapShopGenericRepo<Notification> notificationRepo,
            IHubContext<NotificationHub> hubContext,
            ILogger<NotificationService> logger)
        {
            _notificationRepo = notificationRepo;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendNotificationAsync(SendNotificationRequest request)
        {
            try
            {
                // 1. Persist
                var notification = await _notificationRepo.Add(new Notification
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Message = request.Message,
                    Type = request.Type,
                    ReferenceId = request.ReferenceId
                });
                await _notificationRepo.SaveChanges();

                // 2. Push real-time to the user's group
                var payload = new NotificationDto
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    Type = notification.Type,
                    ReferenceId = notification.ReferenceId,
                    IsRead = false,
                    CreatedAt = notification.Created
                };

                await _hubContext.Clients
                    .Group($"user_{request.UserId}")
                    .SendAsync("ReceiveNotification", payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification to user {UserId}", request.UserId);
            }
        }

        public async Task<ResponseDto<PaginatedResult<NotificationDto>>> GetUserNotificationsAsync(
            string userId, bool? unreadOnly, int pageNumber, int pageSize)
        {
            var response = new ResponseDto<PaginatedResult<NotificationDto>>();
            try
            {
                var page = pageNumber > 0 ? pageNumber : 1;
                var size = pageSize > 0 ? pageSize : 10;

                var query = _notificationRepo.GetQueryable()
                    .AsNoTracking()
                    .Where(n => n.UserId == userId);

                if (unreadOnly == true)
                    query = query.Where(n => !n.IsRead);

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderByDescending(n => n.Created)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(n => new NotificationDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        Type = n.Type,
                        ReferenceId = n.ReferenceId,
                        IsRead = n.IsRead,
                        CreatedAt = n.Created
                    })
                    .ToListAsync();

                response.Result = new PaginatedResult<NotificationDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = size,
                    TotalPages = (int)Math.Ceiling((double)totalCount / size)
                };
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications for user {UserId}", userId);
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                response.ErrorMessages = new List<string> { "Error fetching notifications" };
            }
            return response;
        }

        public async Task<ResponseDto<string>> MarkAsReadAsync(string notificationId, string userId)
        {
            var response = new ResponseDto<string>();
            try
            {
                var notification = await _notificationRepo.GetQueryable()
                    .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

                if (notification == null)
                {
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string> { "Notification not found" };
                    return response;
                }

                notification.IsRead = true;
                _notificationRepo.Update(notification);
                await _notificationRepo.SaveChanges();

                response.Result = "Notification marked as read";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification {Id} as read", notificationId);
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                response.ErrorMessages = new List<string> { "Error marking notification as read" };
            }
            return response;
        }

        public async Task<ResponseDto<string>> MarkAllAsReadAsync(string userId)
        {
            var response = new ResponseDto<string>();
            try
            {
                var unread = await _notificationRepo.GetQueryable()
                    .Where(n => n.UserId == userId && !n.IsRead)
                    .ToListAsync();

                foreach (var n in unread)
                {
                    n.IsRead = true;
                    _notificationRepo.Update(n);
                }

                if (unread.Any())
                    await _notificationRepo.SaveChanges();

                response.Result = $"{unread.Count} notification(s) marked as read";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications as read for user {UserId}", userId);
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                response.ErrorMessages = new List<string> { "Error marking all notifications as read" };
            }
            return response;
        }

        public async Task<ResponseDto<int>> GetUnreadCountAsync(string userId)
        {
            var response = new ResponseDto<int>();
            try
            {
                var count = await _notificationRepo.GetQueryable()
                    .CountAsync(n => n.UserId == userId && !n.IsRead);

                response.Result = count;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count for user {UserId}", userId);
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                response.ErrorMessages = new List<string> { "Error fetching unread count" };
            }
            return response;
        }

        public async Task<ResponseDto<PaginatedResult<NotificationDto>>> GetAllNotificationsAsync(
            string? userId, string? type, int pageNumber, int pageSize)
        {
            var response = new ResponseDto<PaginatedResult<NotificationDto>>();
            try
            {
                var page = pageNumber > 0 ? pageNumber : 1;
                var size = pageSize > 0 ? pageSize : 20;

                var query = _notificationRepo.GetQueryable().AsNoTracking();

                if (!string.IsNullOrWhiteSpace(userId))
                    query = query.Where(n => n.UserId == userId);

                if (!string.IsNullOrWhiteSpace(type))
                    query = query.Where(n => n.Type == type);

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderByDescending(n => n.Created)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(n => new NotificationDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        Type = n.Type,
                        ReferenceId = n.ReferenceId,
                        IsRead = n.IsRead,
                        CreatedAt = n.Created
                    })
                    .ToListAsync();

                response.Result = new PaginatedResult<NotificationDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = size,
                    TotalPages = (int)Math.Ceiling((double)totalCount / size)
                };
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all notifications");
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                response.ErrorMessages = new List<string> { "Error fetching notifications" };
            }
            return response;
        }
    }
}
