using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Auth
{
    public class ResendConfirmationEmailCommand : IRequest<ResponseDto<string>>
    {
        public string Email { get; set; } = string.Empty;
    }
}