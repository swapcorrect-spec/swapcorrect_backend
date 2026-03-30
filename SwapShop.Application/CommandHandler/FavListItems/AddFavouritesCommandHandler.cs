using MediatR;
using SwapShop.Application.Commands.FavListItems;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.CommandHandler.FavListItems
{
    public class AddFavouritesCommandHandler : IRequestHandler<AddFavouritesCommand, ResponseDto<string>>
    {
        private readonly IFavListItemService _favListItem;
        public AddFavouritesCommandHandler(IFavListItemService favListItem)
        {
            _favListItem = favListItem;
        }
        public async Task<ResponseDto<string>> Handle(AddFavouritesCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<string>
            {
                ErrorMessages = new List<string>() // Initialize the error list
            };


            var result = await _favListItem.AddToFavoritesAsync(request.userId, request.listingId);

            if (result != null)
            {
                response.StatusCode = 200; // Or 201 for Created
                response.DisplayMessage = "Item successfully added to favorites";
                response.Result = request.listingId;
            }
            else
            {
                response.StatusCode = 400;
                response.DisplayMessage = "Failed to add item to favorites";
                response.ErrorMessages.Add("The operation completed but returned false");
            }
            return response;
        }
    }
}
