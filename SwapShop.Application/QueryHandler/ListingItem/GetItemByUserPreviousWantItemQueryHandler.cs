using MediatR;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.ListingItem
{
    public class GetItemByUserPreviousWantItemQueryHandler : IRequestHandler<GetItemByUserPreviousWantItemQuery, ResponseDto<List<ListedItemResp>>>
    {
        private readonly IListItemService _listItemService;
        public GetItemByUserPreviousWantItemQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        async Task<ResponseDto<List<ListedItemResp>>> IRequestHandler<GetItemByUserPreviousWantItemQuery, ResponseDto<List<ListedItemResp>>>.Handle(GetItemByUserPreviousWantItemQuery request, CancellationToken cancellationToken)
        {
            return await _listItemService.GetItemByUserPreviousWantItem(request.UserId, request.limit);
        }
    }
}
