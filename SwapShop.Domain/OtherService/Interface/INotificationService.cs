using SwapShop.Domain.Dtos.Request.Notification;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Notification;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface INotificationService
    {
        /// <summary>Persist + push a real-time notification to a user.</summary>
        Task SendNotificationAsync(SendNotificationRequest request);

        /// <summary>Get paginated notifications for a user.</summary>
        Task<ResponseDto<PaginatedResult<NotificationDto>>> GetUserNotificationsAsync(
            string userId, bool? unreadOnly, int pageNumber, int pageSize);

        /// <summary>Mark one notification as read.</summary>
        Task<ResponseDto<string>> MarkAsReadAsync(string notificationId, string userId);

        /// <summary>Mark all notifications as read for a user.</summary>
        Task<ResponseDto<string>> MarkAllAsReadAsync(string userId);

        /// <summary>Unread count badge helper.</summary>
        Task<ResponseDto<int>> GetUnreadCountAsync(string userId);

        /// <summary>Admin: get all notifications across the platform, optionally filtered by userId.</summary>
        Task<ResponseDto<PaginatedResult<NotificationDto>>> GetAllNotificationsAsync(
            string? userId, string? type, int pageNumber, int pageSize);
    }
}
