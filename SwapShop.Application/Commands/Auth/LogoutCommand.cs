using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Auth
{
    public class LogoutCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
    }
}
