using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands
{
    public class ForgetPasswordCommand : IRequest<ResponseDto<string>>
    {
        public string Email { get; set; }
    }
}
