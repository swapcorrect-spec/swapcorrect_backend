using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enum;


namespace SwapShop.Application.Queries.Admin
{
    public class GetUsersQuery : IRequest<ResponseDto<PaginatedResult<UserListDto>>>
    {
        public PaginationFilterDto Filter { get; set; } = new PaginationFilterDto();
        public AdminFilterType FilterType { get; set; } = AdminFilterType.All;
        public int Days { get; set; } = 30;
    }
}
