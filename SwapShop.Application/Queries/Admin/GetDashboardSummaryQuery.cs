using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Queries.Admin
{
    public class GetDashboardSummaryQuery : IRequest<ResponseDto<DashboardSummaryDto>>
    {
        public PeriodicFilter filter {  get; set; } = PeriodicFilter.AllTime;
    }
}
