namespace SwapShop.Domain.Dtos.Request.Notification
{
    public class SendNotificationRequest
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "General";
        public string? ReferenceId { get; set; }
    }
}
