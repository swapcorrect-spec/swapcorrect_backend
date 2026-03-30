namespace SwapShop.Domain.Dtos.Request.ListingItem
{
    public class UpdateListingReq
    {
        public string ListId { get; set; }
        public string ListType { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string EstimatedCurrency { get; set; } = string.Empty;
        public double EstimatedAmount { get; set; }
        public string ItemDescription { get; set; } = string.Empty;
        public string CategoryId { get; set; }
        public string ItemCondition { get; set; }
        public string Location { get; set; }
        public List<ListItemMedia> ListMediaFiles { get; set; }
        public List<ListingDemandDto> ListingSwapReq { get; set; }
    }
}
