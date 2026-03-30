using MediatR;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.ListItem
{
    public class ListItemCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
        public string ListType { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string EstimatedCurrency { get; set; } = string.Empty;
        public double EstimatedAmount { get; set; }
        public string ItemDescription { get; set; } = string.Empty;
        public string CategoryId { get; set; }
        public string ItemCondition { get; set; }
        public string Location { get; set; }
        public List<ListItemMedia> ListMediaFiles { get; set; }
        public List<ListingDemandDto> ListingSwapReq { get; set; }
    }
}
