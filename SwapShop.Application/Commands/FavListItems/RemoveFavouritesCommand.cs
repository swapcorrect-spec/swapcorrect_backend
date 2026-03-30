using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.FavListItems
{
    public class RemoveFavouritesCommand : IRequest<ResponseDto<string>>
    {
        public string userId { get; set; } = string.Empty;
        public string listingId { get; set; } = string.Empty;
    }
}
 