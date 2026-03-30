using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Auth
{
    public class GoogleLoginCommand : IRequest<ResponseDto<LoginResultDto>>
    {
        public string IdToken { get; set; }
       // public string GenericPassword { get; set; }
    }
}
