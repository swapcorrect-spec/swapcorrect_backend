using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enitities;

namespace SwapShop.Application.Queries.ListingItem
{
    public class GetAllCategoryQuery : IRequest<ResponseDto<List<ItemCategory>>>
    {
    }
}
