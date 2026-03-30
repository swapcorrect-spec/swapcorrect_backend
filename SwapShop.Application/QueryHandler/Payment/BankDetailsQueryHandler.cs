using MediatR;
using SwapShop.Application.Queries.Payment;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Payment
{
    public class BankDetailsQueryHandler : IRequestHandler<BankDetailsQuery, ResponseDto<List<PaystackBank>>>
    {
        private readonly IPaymentService _paymentService;

        public BankDetailsQueryHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public async Task<ResponseDto<List<PaystackBank>>> Handle(BankDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _paymentService.BankDetails();
        }
    }
}
