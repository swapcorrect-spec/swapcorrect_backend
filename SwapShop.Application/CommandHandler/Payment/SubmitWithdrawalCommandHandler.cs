using MediatR;
using SwapShop.Application.Commands.Payment;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Payment
{
    public class SubmitWithdrawalCommandHandler : IRequestHandler<SubmitWithdrawalCommand, ResponseDto<string>>
    {
        private readonly IPaymentService _paymentService;

        public SubmitWithdrawalCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<string>> Handle(SubmitWithdrawalCommand request, CancellationToken cancellationToken)
        {
            return await _paymentService.SubmitWithdrawal(request.UserId, request.SwapId);
        }
    }
}
