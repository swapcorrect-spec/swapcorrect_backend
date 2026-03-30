using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;


namespace SwapShop.Application.Queries.Admin
{
    public class AdvancedAnalyticsQuery : IRequest<ResponseDto<AdvancedAnalyticsDto>>
    {
    }
}
