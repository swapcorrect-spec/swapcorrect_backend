using SwapShop.Domain.Enitities;

namespace Swap_Shop.Domain.Entities
{
    public class Saved_Item : BaseEntity
    {
        public string ItemId { get; set; }
        public ListingItem Item { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
