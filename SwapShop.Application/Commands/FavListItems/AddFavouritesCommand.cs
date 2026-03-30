using MediatR;
using SwapShop.Domain.Dtos.Response;
using System;


namespace SwapShop.Application.Commands.FavListItems
{
    public class AddFavouritesCommand : IRequest<ResponseDto<string>>
    {
        public string userId { get; set; } = string.Empty;
        public string listingId { get; set; } = string.Empty;
    }
}
