

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class AdvancedAnalyticsDto
    {
        public List<AnalyticsMetricDto> Metrics { get; set; } = new();
        public Dictionary<string, int> MonthlySwaps { get; set; } = new(); // e.g. Jan–Aug counts
    }
}
