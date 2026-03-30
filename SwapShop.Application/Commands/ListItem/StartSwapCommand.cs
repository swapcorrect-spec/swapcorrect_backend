using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.ListItem
{
    public class StartSwapCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
        public string ListingId { get; set; }
    }
}
