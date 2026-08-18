using MediatR;
using SwapShop.Application.Queries.Payment;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Payment
{
    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, ResponseDto<PaginatedResult<TransactionDto>>>
    {
        private readonly IPaymentService _paymentService;

        public GetAllTransactionsQueryHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<PaginatedResult<TransactionDto>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _paymentService.GetAllTransactions(request.UserId, request.SearchParam, request.DateFilter, request.PageNumber, request.PageSize);
        }
    }
}
