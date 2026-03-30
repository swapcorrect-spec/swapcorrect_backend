using MediatR;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.QueryHandler.ListingItem
{
    public class GetSingleListingQueryHandler : IRequestHandler<GetSingleListingQuery, ResponseDto<ListedItemResp>>
    {
        private readonly IListItemService _listItemService;
        public GetSingleListingQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<ListedItemResp>> Handle(GetSingleListingQuery request, CancellationToken cancellationToken)
        {
          return await _listItemService.GetSingleListing(request.UserId, request.ListingId);
        }
    }
}
