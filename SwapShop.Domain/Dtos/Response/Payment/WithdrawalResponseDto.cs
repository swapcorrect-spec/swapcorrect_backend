namespace SwapShop.Domain.Dtos.Response.Payment
{
    public class WithdrawalResponseDto
    {
        public string WithdrawalId { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string SwapId { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string? AdminNote { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
