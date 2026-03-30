using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands
{
    public class DeleteUserCommand : IRequest<ResponseDto<string>>
    {
        public string id { get; set; } = string.Empty;
    }
}
