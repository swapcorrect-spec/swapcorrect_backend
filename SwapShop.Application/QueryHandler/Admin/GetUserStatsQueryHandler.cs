using MediatR;
using SwapShop.Application.Queries.Admin;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Admin
{
    public class GetUserStatsQueryHandler : IRequestHandler<GetUserStatsQuery, ResponseDto<UserStatsDto>>
    {
        private readonly IAdminService _adminService;

        public GetUserStatsQueryHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<ResponseDto<UserStatsDto>> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
        {
            var result = await _adminService.GetUserStatsAsync(cancellationToken);
            return new ResponseDto<UserStatsDto>
            {
                StatusCode = 200,
                DisplayMessage = "User stats fetched successfully",
                Result = result
            };
        }
    }
}
