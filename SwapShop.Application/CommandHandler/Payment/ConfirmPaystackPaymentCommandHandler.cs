using MediatR;
using SwapShop.Application.Commands.Payment;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Payment
{
    public class ConfirmPaystackPaymentCommandHandler : IRequestHandler<ConfirmPaystackPaymentCommand, ResponseDto<string>>
    {
        private readonly IPaymentService _paymentService;

        public ConfirmPaystackPaymentCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<string>> Handle(ConfirmPaystackPaymentCommand request, CancellationToken cancellationToken)
        {
            return await _paymentService.ConfirmPaystackpayment(request.Reference, request.UserId);
        }
    }
}
