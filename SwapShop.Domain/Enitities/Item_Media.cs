using SwapShop.Domain.Enitities;

namespace Swap_Shop.Domain.Entities
{
    public class Item_Media : BaseEntity
    {
        public string ItemId { get; set; }
        public string MediaType { get; set; }
        public string Url { get; set; }
        public ListingItem Item { get; set; }
    }
}
