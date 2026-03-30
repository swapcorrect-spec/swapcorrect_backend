using Swap_Shop.Domain.Entities;

namespace SwapShop.Domain.Enitities
{
    public class BankAccount : BaseEntity
    {
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string BankCode { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
    }
}
