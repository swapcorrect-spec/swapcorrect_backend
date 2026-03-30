namespace SwapShop.Domain.Dtos.Response
{
    public class UserStatisticsResponse
    {
        public List<UserStatistics> UserStatistics { get; set; }
        public int CurrentYearTotalUserCount { get; set; }
        public int TotalUserCount { get; set; }
    }
}
