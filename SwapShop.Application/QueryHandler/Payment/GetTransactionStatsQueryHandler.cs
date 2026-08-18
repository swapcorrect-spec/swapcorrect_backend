using MediatR;
using SwapShop.Application.Queries.Payment;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Payment
{
    public class GetTransactionStatsQueryHandler : IRequestHandler<GetTransactionStatsQuery, ResponseDto<TransactionStatsDto>>
    {
        private readonly IPaymentService _paymentService;

        public GetTransactionStatsQueryHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<TransactionStatsDto>> Handle(GetTransactionStatsQuery request, CancellationToken cancellationToken)
        {
            return await _paymentService.GetTransactionStats();
        }
    }
}
