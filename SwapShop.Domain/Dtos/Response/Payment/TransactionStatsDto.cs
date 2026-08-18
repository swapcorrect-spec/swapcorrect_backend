namespace SwapShop.Domain.Dtos.Response.Payment
{
    public class TransactionStatsDto
    {
        // Overall totals
        public int TotalTransactions { get; set; }
        public decimal TotalRevenue { get; set; }

        // By status
        public int SuccessCount { get; set; }
        public decimal SuccessRevenue { get; set; }
        public int PendingCount { get; set; }
        public decimal PendingRevenue { get; set; }
        public int FailedCount { get; set; }
        public decimal FailedRevenue { get; set; }

        // By period
        public int TodayCount { get; set; }
        public decimal TodayRevenue { get; set; }
        public int ThisWeekCount { get; set; }
        public decimal ThisWeekRevenue { get; set; }
        public int ThisMonthCount { get; set; }
        public decimal ThisMonthRevenue { get; set; }

        // Breakdowns
        public List<TransactionGroupStatDto> ByFeeType { get; set; } = new();
        public List<TransactionGroupStatDto> ByPaymentType { get; set; } = new();
        public List<TransactionGroupStatDto> ByPaymentChannel { get; set; } = new();
    }

    public class TransactionGroupStatDto
    {
        public string Label { get; set; }
        public int Count { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
