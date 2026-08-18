using MediatR;
using SwapShop.Application.Queries.Admin;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Admin
{
    internal class AdminSearchpaginatedListingQueryHandler
    : IRequestHandler<AdminSearchpaginatedListingQuery, ResponseDto<PaginatedResult<ListedItemResp>>>
    {
        private readonly IListItemService _listItemService;

        public AdminSearchpaginatedListingQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<PaginatedResult<ListedItemResp>>> Handle(AdminSearchpaginatedListingQuery request, CancellationToken cancellationToken)
        {
            return await _listItemService.AdminSearchpaginatedListing(request.UserId, request.listingUserId, request.searhParam,
                request.categoryId, request.location, request.swapListingStatus, request.reviewStage, request.lowestRange, request.highestRange, request.listingDateType, request.pageNumber, request.perpageSize);
        }
    }
}
