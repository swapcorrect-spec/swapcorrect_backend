using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;

namespace Swap_Shop.Domain.Entities
{
    public class ListingItem : BaseEntity
    {
        public string ListType { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string EstimatedCurrency { get; set; } = string.Empty;
        public string Location { get; set; }
        public double EstimatedAmount  { get; set; } 
   
        public string ItemDescription { get; set; } = string.Empty;
        public string CategoryId { get; set; }
        public string ReviewStage { get; set; } = ListingReiviewStage.Pending.ToString();
        public string SwapListStatus { get; set; } = SwapListingStatus.Published.ToString();
        public ItemCategory Category { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public ICollection<FavListItem> favListItems { get; set; }
        public ICollection<Item_Media> Media { get; set; }
        public ICollection<SwapListRequest> SwapListRequest { get; set; }
        public string ItemCondition { get; set; } = string.Empty;
    }
}
