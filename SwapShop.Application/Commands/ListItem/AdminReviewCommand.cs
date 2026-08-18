using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enum;

namespace SwapShop.Application.Commands.ListItem
{
    public class AdminReviewCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
        public string ListingId { get; set; }
        public ListingReiviewStage review { get; set; }
        public string? RejectionNote { get; set; }
    }
}
