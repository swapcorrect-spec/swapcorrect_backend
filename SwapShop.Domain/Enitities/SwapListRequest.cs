using Swap_Shop.Domain.Entities;

namespace SwapShop.Domain.Enitities
{
    public class SwapListRequest : BaseEntity
    {
        public string ItemNeededName { get; set; }
        public string ListedItemId { get; set; }
        public ListingItem ListedItem { get; set; }
       
    }
}
