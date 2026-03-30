using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.Enum;

namespace SwapShop.Application.Queries.ListingItem
{
    public class SearchpaginatedListingQuery : IRequest<ResponseDto<PaginatedResult<ListedItemResp>>>
    {
        public string? UserId { get; set; }
        public string? searhParam { get; set; }
        public string? listingUserId { get; set; }
        public string? categoryId { get; set; }
        public string? location { get; set; }
        public decimal lowestRange { get; set; }
        public decimal highestRange { get; set; }
        public ListingDateFilter listingDateType { get; set; }
        public int pageNumber { get; set; }
        public int perpageSize { get; set; }
    }
}
