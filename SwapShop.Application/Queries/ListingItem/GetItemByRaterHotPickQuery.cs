using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;

namespace SwapShop.Application.Queries.ListingItem
{
    public class GetItemByRaterHotPickQuery : IRequest<ResponseDto<List<ListedItemResp>>>
    {
        public string? UserId { get; set; }
        public int limit { get; set; }
    }
}
