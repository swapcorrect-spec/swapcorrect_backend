using MediatR;
using SwapShop.Application.Commands.Payment;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Payment
{
    public class IntializePaymentForPaystackCommandHandler : IRequestHandler<IntializePaymentForPaystackCommand, ResponseDto<PaystackInitializeResponse>>
    {
        private readonly IPaymentService _paymentService;

        public IntializePaymentForPaystackCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public async Task<ResponseDto<PaystackInitializeResponse>> Handle(IntializePaymentForPaystackCommand request, CancellationToken cancellationToken)
        {
            return await _paymentService.IntializePaymentForPaystack(request.ProceedingId, request.UserId, request.IsChargeFee,request.RoomName);
        }
    }
}
