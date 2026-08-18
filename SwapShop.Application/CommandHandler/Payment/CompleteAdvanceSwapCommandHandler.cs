using MediatR;
using SwapShop.Application.Commands.Payment;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Payment
{
    public class CompleteAdvanceSwapCommandHandler : IRequestHandler<CompleteAdvanceSwapCommand, ResponseDto<string>>
    {
        private readonly IPaymentService _paymentService;

        public CompleteAdvanceSwapCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<string>> Handle(CompleteAdvanceSwapCommand request, CancellationToken cancellationToken)
        {
            return await _paymentService.CompleteAdvanceSwap(request.UserId, request.SwapId);
        }
    }
}
