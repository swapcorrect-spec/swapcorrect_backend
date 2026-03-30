using MediatR;
using SwapShop.Application.Commands.FavListItems;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;
using System;

namespace SwapShop.Application.CommandHandler.FavListItems
{
    public class RemoveFavouritesCommandHandler : IRequestHandler<RemoveFavouritesCommand, ResponseDto<string>>
    {
        private readonly IFavListItemService _favListItem;
        public RemoveFavouritesCommandHandler(IFavListItemService favListItem)
        {
            _favListItem = favListItem;
        }

        public async Task<ResponseDto<string>> Handle(RemoveFavouritesCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<string>
            {
                ErrorMessages = new List<string>()
            };

            var result = await _favListItem.RemoveFromFavouritesAsync(request.userId, request.listingId);

            if(result != null)
            {
                response.StatusCode = 200; // Or 201 for Created
                response.DisplayMessage = "Item successfully removed from favourites";
                response.Result = request.listingId;
            }
            else
            {
                response.StatusCode = 400;
                response.DisplayMessage = "Failed to remove item from favourites";
                response.ErrorMessages.Add("The operation completed but returned false");
            }
            return response;

        }
    }
}
