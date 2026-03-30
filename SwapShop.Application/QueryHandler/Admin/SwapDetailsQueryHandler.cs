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
    public class SwapDetailsQueryHandler : IRequestHandler<SwapDetailsQuery, ResponseDto<SwapDetailsDto>>
    {
        private readonly IAdminService _adminService;
        public SwapDetailsQueryHandler(IAdminService adminService)
        {
            _adminService = adminService;
            
        }
        public async Task<ResponseDto<SwapDetailsDto>> Handle(SwapDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _adminService.GetSwapDetailsAsync(request.SwapId);
            return new ResponseDto<SwapDetailsDto>
            {
                StatusCode = 200,
                DisplayMessage = "Advanced Analytics fetched successfully",
                Result = result
            };
        }
    }
}
