namespace SwapShop.Domain.Dtos.Response.ListingItem
{
    public class UserDashboardCard
    {
        public int ListedCount { get; set; }
        public int OngoingCount { get; set; }
        public int PendingConfirmationCount { get; set; }
        public int CompletedCount { get; set; }
    }
}
