using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.Dtos.Response.Report;
using SwapShop.Domain.Enum;

namespace SwapShop.Application.Queries.Report
{
    public class SearchUserReportPaginatedQuery : IRequest<ResponseDto<PaginatedResult<UserReportPaginatedDto>>>
    {
        public string? searhParam { get; set; }
        public ReportUserStatus status { get; set; }
        public ReportDateFilter reportFilerDate { get; set; }
        public string? UserId { get; set; }
        public int pageNumber { get; set; }
        public int perpageSize { get; set; }
    }
}
