using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enum;

namespace SwapShop.Application.Commands.Admin
{
    public class FlagContentCommand : IRequest<ResponseDto<string>>
    {
        public string ContentId { get; set; }
        public FlagContentType ContentType { get; set; }
        public bool IsFlagged { get; set; } = true;
    }
}
