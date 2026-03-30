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
    public class SwapsActivityQueryHandler : IRequestHandler<SwapsActivityQuery, ResponseDto<PaginatedResult<SwapActivityDto>>>
    {
        private readonly IAdminService _adminService;
        public SwapsActivityQueryHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<ResponseDto<PaginatedResult<SwapActivityDto>>> Handle(SwapsActivityQuery request, CancellationToken cancellationToken)
        {
            var result = await _adminService.GetSwapsActivityAsync(request.PageNumber, request.PageSize, request.filter);

            return new ResponseDto<PaginatedResult<SwapActivityDto>>
            {
                StatusCode = 200,
                DisplayMessage = "Swaps fetched successfully",
                Result = result
            };
        }
    }
}
