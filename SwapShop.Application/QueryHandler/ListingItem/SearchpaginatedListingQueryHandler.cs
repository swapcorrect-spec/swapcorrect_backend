using MediatR;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.ListingItem
{
    public class SearchpaginatedListingQueryHandler : IRequestHandler<SearchpaginatedListingQuery, ResponseDto<PaginatedResult<ListedItemResp>>>
    {
        private readonly IListItemService _listItemService;

        public SearchpaginatedListingQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }

        public async Task<ResponseDto<PaginatedResult<ListedItemResp>>> Handle(SearchpaginatedListingQuery request, CancellationToken cancellationToken)
        {
            return await _listItemService.SearchpaginatedListing(request.UserId,request.listingUserId ,request.searhParam, request.categoryId, 
                request.location, request.lowestRange, request.highestRange, request.listingDateType, request.pageNumber, request.perpageSize);
        }
    }
}
