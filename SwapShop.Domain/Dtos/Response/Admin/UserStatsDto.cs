namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class UserStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int TotalSwappers { get; set; }
        public int TotalVisitors { get; set; }
    }
}
