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
    
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ResponseDto<PaginatedResult<UserListDto>>>
    {
        private readonly IAdminService _adminService;
        public GetUsersQueryHandler(IAdminService adminService)
        {
            _adminService = adminService;
            
        }
        public async Task<ResponseDto<PaginatedResult<UserListDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _adminService.GetUsersAsync(request.Filter, request.FilterType, request.Days);
            return new ResponseDto<PaginatedResult<UserListDto>>
            {
                StatusCode = 200,
                DisplayMessage = $"{request.FilterType} users fetched successfully",
                Result = result
            };
        }
    }
}
