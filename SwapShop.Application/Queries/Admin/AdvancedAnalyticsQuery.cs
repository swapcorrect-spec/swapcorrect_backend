using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enum;


namespace SwapShop.Application.Queries.Admin
{
    public class AdvancedAnalyticsQuery : IRequest<ResponseDto<AdvancedAnalyticsDto>>
    {
        public AnalyticsMetricFilter MetricFilter { get; set; } = AnalyticsMetricFilter.All;
        public PeriodicFilter PeriodicFilter { get; set; } = PeriodicFilter.AllTime;
    }
}
