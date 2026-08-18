using Swap_Shop.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapShop.Domain.Enitities
{
    public class WithdrawalRequest : BaseEntity
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public string SwapId { get; set; }
        public SwappingProceeding Swap { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; } = "Pending";
        public string? AdminNote { get; set; }
    }
}
