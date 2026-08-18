using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Auth
{
    public class RefreshTokenCommand : IRequest<ResponseDto<LoginResultDto>>
    {
        public string RefreshToken { get; set; }
    }
}
