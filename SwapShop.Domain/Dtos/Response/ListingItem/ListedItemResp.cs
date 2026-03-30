using SwapShop.Domain.Dtos.Response.FavListItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.ListingItem
{
    public class ListedItemResp
    {
        public string ListingId { get; set; }
        public string UserId { get; set; }
        public string ListType { get; set; }
        public string ItemName { get; set; }
        public string EstimatedCurrency { get; set; }
        public double Rating { get; set; }
        public int SwapCount { get; set; }
        public double EstimatedAmount { get; set; }
        public string ItemDescription { get; set; }
        public bool IsFavItem { get; set; }
        public string ReviewStage { get; set; }
        public string CategoryName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string Username { get; set; }
     
        public List<ImageItemDto> Media { get; set; }
        public List<string> SwapListRequest { get; set; }
        public string ItemCondition { get; set; } = string.Empty;
    }
}
