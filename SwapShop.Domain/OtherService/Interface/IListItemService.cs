using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IListItemService
    {
        Task<ResponseDto<ListedItemResp>> GetSingleListing(string? userId, string listingId);
        Task<ResponseDto<List<ListedItemResp>>> GetItemByRaterHotPick(string? userId, int limit);
        Task<ResponseDto<string>> CreateItemRequest(ListItemReq req, string userid);
        Task<ResponseDto<string>> AdminReview(string userId, string listId, ListingReiviewStage review, string? rejectionNote = null);
        Task<ResponseDto<List<ListedItemResp>>> GetItemByUserPreviousWantItem(string? userId, int limit);
        Task<ResponseDto<List<ListedItemResp>>> GetItemElectronics(string? userId, int limit);
        Task<ResponseDto<List<ItemCategory>>> GetAllCategory();
        Task<ResponseDto<string>> CloseSwap(string userId, string listId);
        Task<ResponseDto<string>> UpdateListingItemRequest(UpdateListingReq req, string userid);
        Task<ResponseDto<PaginatedResult<ListedItemResp>>> SearchpaginatedListing(string? userId, string? listinguserId, string? searhParam, string? categoryId,
            string? location, decimal lowestRange, decimal highestRange, ListingDateFilter listingDate, int pageNumber, int perpageSize);
        Task<ResponseDto<string>> SwitchSwapStatus(string userId, string listId, SwapProceedingStatus swapProceeding);
        Task<ResponseDto<string>> RemoveSingleListing(string? userId, string listingId);
        Task<ResponseDto<UserDashboardCard>> GetUserDashboardCard(string userId);
        Task<ResponseDto<string>> StartSwap(string userId, string listId);
        Task<ResponseDto<SwapProceedingResp>> SingleListingSwapProceed(string swapProceedId);
        Task<ResponseDto<PaginatedResult<SwapProceedingResp>>> SearchpaginatedListingSwap(string? listinguserId, string? searhParam,
            SwapListingEnumStatus swapListingStatus, ListingDateFilter listingDate, int pageNumber, int perpageSize);
        Task<ResponseDto<PaginatedResult<ListedItemResp>>> AdminSearchpaginatedListing(string? userId, string? listinguserId, string? searhParam,
           string? categoryId, string? location, SwapListingStatus listingStatus, ListingReiviewStage reviewStage, decimal lowestRange, decimal highestRange, ListingDateFilter listingDate, int pageNumber, int perpageSize);
        Task<ResponseDto<string>> FlagContent(string contentId, FlagContentType contentType, bool isFlagged);
    }
}
