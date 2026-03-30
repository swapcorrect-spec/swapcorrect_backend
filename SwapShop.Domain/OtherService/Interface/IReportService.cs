using SwapShop.Domain.Dtos.Request.Report;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Report;
using SwapShop.Domain.Enum;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IReportService
    {
        Task<ResponseDto<string>> AddReportReportAdminNoted(string Note, string ReportId);
        Task<ResponseDto<string>> ReportUser(ReportUserDto req, string userid);
        Task<ResponseDto<string>> ChangeReportStatus(ReportUserStatus status, string ReportId);
        Task<ResponseDto<PaginatedResult<UserReportPaginatedDto>>> SearchUserReportPaginated(string? searhParam, ReportUserStatus status,
           ReportDateFilter reportFilerDate, int pageNumber, int perpageSize);
        Task<ResponseDto<SingleReportDetails>> GetSingleReportDetails(string ReportId);
    }
}
