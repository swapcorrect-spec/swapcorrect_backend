using MediatR;
using SwapShop.Application.Queries.Report;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Report;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Report
{
    public class SearchUserReportPaginatedQueryHandler : IRequestHandler<SearchUserReportPaginatedQuery, ResponseDto<PaginatedResult<UserReportPaginatedDto>>>
    {
        private readonly IReportService _reportService;

        public SearchUserReportPaginatedQueryHandler(IReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<ResponseDto<PaginatedResult<UserReportPaginatedDto>>> Handle(SearchUserReportPaginatedQuery request, CancellationToken cancellationToken)
        {
            return await _reportService.SearchUserReportPaginated(request.searhParam, request.status, request.reportFilerDate, request.UserId, request.pageNumber, request.perpageSize);
        }
    }
}
