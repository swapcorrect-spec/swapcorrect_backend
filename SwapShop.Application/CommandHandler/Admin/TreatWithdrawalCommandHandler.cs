using MediatR;
using SwapShop.Application.Commands.Admin;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Admin
{
    public class TreatWithdrawalCommandHandler : IRequestHandler<TreatWithdrawalCommand, ResponseDto<string>>
    {
        private readonly IPaymentService _paymentService;

        public TreatWithdrawalCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<string>> Handle(TreatWithdrawalCommand request, CancellationToken cancellationToken)
        {
            return await _paymentService.TreatWithdrawal(request.WithdrawalId, request.AdminNote);
        }
    }
}
