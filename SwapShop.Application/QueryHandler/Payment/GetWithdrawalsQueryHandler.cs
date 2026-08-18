using MediatR;
using SwapShop.Application.Queries.Payment;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Payment
{
    public class GetWithdrawalsQueryHandler : IRequestHandler<GetWithdrawalsQuery, ResponseDto<PaginatedResult<WithdrawalResponseDto>>>
    {
        private readonly IPaymentService _paymentService;

        public GetWithdrawalsQueryHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<PaginatedResult<WithdrawalResponseDto>>> Handle(GetWithdrawalsQuery request, CancellationToken cancellationToken)
        {
            return await _paymentService.GetWithdrawals(request.UserId, request.Status, request.PageNumber, request.PageSize);
        }
    }
}
