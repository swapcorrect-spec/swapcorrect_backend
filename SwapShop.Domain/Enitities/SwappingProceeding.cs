using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Enum;

namespace SwapShop.Domain.Enitities
{
    public class SwappingProceeding : BaseEntity
    {
        public string ListId { get; set; }
        public string Userid { get; set; }
        public string Status { get; set; } = SwapProceedingStatus.Negotiation.ToString();
        public ApplicationUser User { get; set; }
        public ListingItem List { get; set; }
    }
}
