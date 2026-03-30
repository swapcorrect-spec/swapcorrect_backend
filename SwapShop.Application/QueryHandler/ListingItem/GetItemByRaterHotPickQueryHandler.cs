using MediatR;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.ListingItem
{
    public class GetItemByRaterHotPickQueryHandler : IRequestHandler<GetItemByRaterHotPickQuery, ResponseDto<List<ListedItemResp>>>
    {
        private readonly IListItemService _listItemService;

        public GetItemByRaterHotPickQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<List<ListedItemResp>>> Handle(GetItemByRaterHotPickQuery request, CancellationToken cancellationToken)
        {
            return await _listItemService.GetItemByRaterHotPick(request.UserId, request.limit);
        }
    }
}
