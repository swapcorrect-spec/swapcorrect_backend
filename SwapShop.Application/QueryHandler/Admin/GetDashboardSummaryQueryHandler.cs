using MediatR;
using SwapShop.Application.Queries.Admin;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.QueryHandler.Admin
{
    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, ResponseDto<DashboardSummaryDto>>
    {
        private readonly IAdminService _adminService;
        public GetDashboardSummaryQueryHandler(IAdminService adminService)
        {
            _adminService = adminService; 
        }
        public async Task<ResponseDto<DashboardSummaryDto>> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var result = await _adminService.GetDashboardSummaryAsync(request.filter);
            return new ResponseDto<DashboardSummaryDto>
            {
                StatusCode = 200,
                DisplayMessage = $"Display {request.filter} result successful",
                Result = result
            };
        }
    }
}
