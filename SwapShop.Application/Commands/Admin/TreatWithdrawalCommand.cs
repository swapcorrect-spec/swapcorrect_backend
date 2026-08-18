using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Admin
{
    public class TreatWithdrawalCommand : IRequest<ResponseDto<string>>
    {
        public string WithdrawalId { get; set; }
        public string? AdminNote { get; set; }
    }
}
