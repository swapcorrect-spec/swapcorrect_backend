using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.Enum;

namespace SwapShop.Application.Queries.Admin
{
    public class AdminSearchpaginatedListingQuery : IRequest<ResponseDto<PaginatedResult<ListedItemResp>>>
    {
        public string? UserId { get; set; }
        public string? searhParam { get; set; }
        public string? listingUserId { get; set; }
        public string? categoryId { get; set; }
        public string? location { get; set; }
        public decimal lowestRange { get; set; }
        public decimal highestRange { get; set; }
        public SwapListingStatus swapListingStatus { get; set; }
        public ListingDateFilter listingDateType { get; set; }
        public int pageNumber { get; set; }
        public int perpageSize { get; set; }
    }
}
