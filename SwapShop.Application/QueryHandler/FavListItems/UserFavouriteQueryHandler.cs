using MediatR;
using SwapShop.Application.Queries.FavListItems;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.FavListItems;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.FavListItems
{
    public class UserFavouriteQueryHandler : IRequestHandler<UserFavouriteQuery, ResponseDto<List<FavListItemResponseDto>>>
    {
        private readonly IFavListItemService _favListItem;
        public UserFavouriteQueryHandler(IFavListItemService favListItem)
        {
            _favListItem = favListItem;
        }

        public async Task<ResponseDto<List<FavListItemResponseDto>>> Handle(UserFavouriteQuery request, CancellationToken cancellationToken)
        {

            return await _favListItem.GetUserFavouritesAsync(request.UserId);

        }
    }
}
