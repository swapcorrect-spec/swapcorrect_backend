using MediatR;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.ListingItem
{
    public class GetItemElectronicsQueryHandler : IRequestHandler<GetItemElectronicsQuery, ResponseDto<List<ListedItemResp>>>
    {
        private readonly IListItemService _listItemService;

        public GetItemElectronicsQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<List<ListedItemResp>>> Handle(GetItemElectronicsQuery request, CancellationToken cancellationToken)
        {
            return await _listItemService.GetItemElectronics(request.UserId, request.limit);
        }
    }
}
