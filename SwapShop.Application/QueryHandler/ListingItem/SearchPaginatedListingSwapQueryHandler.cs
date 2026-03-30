using MediatR;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enum;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.QueryHandler.ListingItem
{
    public class SearchPaginatedListingSwapQueryHandler : IRequestHandler<SearchPaginatedListingSwapQuery, ResponseDto<PaginatedResult<SwapProceedingResp>>>
    {
        private readonly IListItemService _listItemService;
        public SearchPaginatedListingSwapQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<PaginatedResult<SwapProceedingResp>>> Handle(SearchPaginatedListingSwapQuery request, CancellationToken cancellationToken)
        {
            return await _listItemService.SearchpaginatedListingSwap(request.ListingUserId, request.SearhParam, request.SwapListingStatus,
                request.ListingDate, request.PageNumber, request.PerPageSize);  
        }
    }
}
