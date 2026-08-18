namespace SwapShop.Domain.Dtos.Response.Payment
{
    public class TransactionDto
    {
        public string TransactionId { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string Amount { get; set; }
        public string FeeType { get; set; }
        public string PaymentType { get; set; }
        public string PaymentChannel { get; set; }
        public string PaymentStatus { get; set; }
        public string Description { get; set; }
        public string SwapId { get; set; }
        public string RoomName { get; set; }
        public DateTime CreatedPaymentTime { get; set; }
        public DateTime? CompletePaymentTime { get; set; }
    }
}
