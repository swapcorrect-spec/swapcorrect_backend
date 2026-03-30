using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;


namespace SwapShop.Domain.OtherService.Interface
{
    public interface IAdminService
    {
        Task<PaginatedResult<RecentActivityDto>> GetRecentActivitiesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<AdvancedAnalyticsDto> GetAdvancedAnalyticsAsync();
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(PeriodicFilter filter);
        Task<PaginatedResult<UserListDto>> GetUsersAsync(PaginationFilterDto filter, AdminFilterType filterType, int days = 30);
        Task<PaginatedResult<SwapActivityDto>> GetSwapsActivityAsync(int pageNumber, int pageSize, PeriodicFilter filter);
        Task<SwapDetailsDto> GetSwapDetailsAsync(string userRoomId);

        /* Task<DashboardCardDto> TotalSwapperAsync(PeriodicFilter filter);
         Task<DashboardCardDto> TotalVisitorAsync(PeriodicFilter filter);
         Task<PaginatedResult<UserListDto>> GetAllUsersAsync(PaginationFilterDto filter);
         Task<PaginatedResult<UserListDto>> GetFlaggedUsersAsync(PaginationFilterDto filter);
         Task<PaginatedResult<UserListDto>> GetSuspendedUsersAsync(PaginationFilterDto filter);
         Task<PaginatedResult<UserListDto>> GetActiveUsersAsync(PaginationFilterDto filter);
         Task<PaginatedResult<UserListDto>> GetNewSignupsAsync(PaginationFilterDto filter, int days = 30);
        Task<DashboardCardDto> SwapperAndVisitor(DashboardMetricType metricType, PeriodicFilter filter);
         Task<DashboardSummaryDto> SwapperAndVisitor(PeriodicFilter filter);
        Task<DashboardCardDto> GetCompletedSwapsAsync();
        Task<DashboardCardDto> GetActiveSwapsAsync();
        Task<DashboardCardDto> TotalActiveUserAsync();
        Task<DashboardCardDto> TotalRegisterUserAsync()*/

    }
}
