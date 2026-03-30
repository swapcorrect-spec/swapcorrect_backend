using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace ProjectX.Application.Commands.Auth
{
    public class ConfirmEmailCommand : IRequest<ResponseDto<string>>
    {
        public int token { get; set; }
        public string email { get; set; } = string.Empty;
    }
}
