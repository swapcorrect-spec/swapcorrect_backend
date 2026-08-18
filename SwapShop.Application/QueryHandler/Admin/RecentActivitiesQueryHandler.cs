using MediatR;
using SwapShop.Application.Queries.Admin;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.QueryHandler.Admin
{
    public class RecentActivitiesQueryHandler : IRequestHandler<RecentActivitiesQuery, ResponseDto<PaginatedResult<RecentActivityDto>>>
    {
        private readonly IAdminService _adminService;
        public RecentActivitiesQueryHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResponseDto<PaginatedResult<RecentActivityDto>>> Handle(RecentActivitiesQuery request, CancellationToken cancellationToken)
        {
            var recentActivites = await _adminService.GetRecentActivitiesAsync(request.PageNumber, request.PageSize, request.UserId, cancellationToken);
            return new ResponseDto<PaginatedResult<RecentActivityDto>> 
            { 
                StatusCode = 200,
                DisplayMessage = "Recent activities fetched successfully",
                Result = recentActivites
            };
        }
    }
}
