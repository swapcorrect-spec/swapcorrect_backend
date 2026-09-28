using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.ListItem
{
    public class CloseListingCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; } = string.Empty;
        public string ListingId { get; set; } = string.Empty;
    }
}