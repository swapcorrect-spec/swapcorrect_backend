using MediatR;
using SwapShop.Application.Queries.Report;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Report;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Report
{
    public class GetSingleReportDetailsQueryHandler : IRequestHandler<GetSingleReportDetailsQuery, ResponseDto<SingleReportDetails>>
    {
        private readonly IReportService _reportService;

        public GetSingleReportDetailsQueryHandler(IReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<ResponseDto<SingleReportDetails>> Handle(GetSingleReportDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _reportService.GetSingleReportDetails(request.reportId);
        }
    }
}
