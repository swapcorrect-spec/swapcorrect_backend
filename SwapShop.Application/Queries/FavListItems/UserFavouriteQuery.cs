using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.FavListItems;


namespace SwapShop.Application.Queries.FavListItems
{
    public class UserFavouriteQuery : IRequest<ResponseDto<List<FavListItemResponseDto>>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
