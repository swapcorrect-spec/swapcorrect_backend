using MediatR;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.ListingItem
{
    public class SingleSwapProceedingQueryHandler : IRequestHandler<SingleSwapProceedingQuery, ResponseDto<SwapProceedingResp>>
    {
        private readonly IListItemService _listItemService;

        public SingleSwapProceedingQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<SwapProceedingResp>> Handle(SingleSwapProceedingQuery request, CancellationToken cancellationToken)
        {
            return await _listItemService.SingleListingSwapProceed(request.SwapProceedId);
        }
    }
}
