using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.Dtos.Response.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Queries.Report
{
    public class GetSingleReportDetailsQuery : IRequest<ResponseDto<SingleReportDetails>>
    {
       
        public string reportId { get; set; }
    }
}
