using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;

namespace SwapShop.Application.Queries.Payment
{
    public class GetTransactionStatsQuery : IRequest<ResponseDto<TransactionStatsDto>>
    {
    }
}
