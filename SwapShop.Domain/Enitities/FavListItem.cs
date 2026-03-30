using Swap_Shop.Domain.Entities;

namespace SwapShop.Domain.Enitities
{
    public class FavListItem : BaseEntity
    {
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string ListId { get; set; }
        public ListingItem List { get; set; }
    }
}
