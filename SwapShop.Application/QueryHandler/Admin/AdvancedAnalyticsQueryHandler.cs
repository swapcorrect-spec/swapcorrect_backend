using MediatR;
using SwapShop.Application.Queries.Admin;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.OtherService.Interface;


namespace SwapShop.Application.QueryHandler.Admin
{
    public class AdvancedAnalyticsQueryHandler : IRequestHandler<AdvancedAnalyticsQuery, ResponseDto<AdvancedAnalyticsDto>>
    {
        private readonly IAdminService _adminService;

        public AdvancedAnalyticsQueryHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<ResponseDto<AdvancedAnalyticsDto>> Handle(AdvancedAnalyticsQuery request, CancellationToken cancellationToken)
        {
            var result = await _adminService.GetAdvancedAnalyticsAsync(request.MetricFilter, request.PeriodicFilter);

            return new ResponseDto<AdvancedAnalyticsDto>
            {
                StatusCode = 200,
                DisplayMessage = "Advanced Analytics fetched successfully",
                Result = result
            };
        }
    }
}
