using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enum;


namespace SwapShop.Application.Queries.Admin
{
    public class SwapsActivityQuery : IRequest<ResponseDto<PaginatedResult<SwapActivityDto>>>
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public PeriodicFilter filter { get; set; } = PeriodicFilter.AllTime;
    }
}
