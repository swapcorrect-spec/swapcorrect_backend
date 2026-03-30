using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.FavListItems;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IFavListItemService
    {
        Task<ResponseDto<List<FavListItemResponseDto>>> GetUserFavouritesAsync(string userId);
        Task<ResponseDto<string>> RemoveFromFavouritesAsync(string userId, string listingId);
        Task<ResponseDto<string>> AddToFavoritesAsync(string userId, string listingId);
    }
}
