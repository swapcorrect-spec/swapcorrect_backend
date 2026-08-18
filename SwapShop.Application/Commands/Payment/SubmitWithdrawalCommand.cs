using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Payment
{
    public class SubmitWithdrawalCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
        public string SwapId { get; set; }
    }
}
