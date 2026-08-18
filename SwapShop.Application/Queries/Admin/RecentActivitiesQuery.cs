using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Queries.Admin
{
    public class RecentActivitiesQuery : IRequest<ResponseDto<PaginatedResult<RecentActivityDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? UserId { get; set; }
    }
}
