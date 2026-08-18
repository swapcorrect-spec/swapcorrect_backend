namespace SwapShop.Domain.Dtos.Request.Payment
{
    public class SubmitWithdrawalRequest
    {
        public string SwapId { get; set; }
    }

    public class CompleteAdvanceSwapRequest
    {
        public string SwapId { get; set; }
    }
}
