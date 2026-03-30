using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Auth
{
    public class LoginCommand : IRequest<ResponseDto<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
