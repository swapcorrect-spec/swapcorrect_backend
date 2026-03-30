using MediatR;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.ListingItem
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, ResponseDto<List<ItemCategory>>>
    {
        private readonly IListItemService _listItemService;
        public GetAllCategoryQueryHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<List<ItemCategory>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _listItemService.GetAllCategory();
        }
    }
}
