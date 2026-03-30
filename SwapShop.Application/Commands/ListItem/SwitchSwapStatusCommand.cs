using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enum;


namespace SwapShop.Application.Commands.ListItem
{
    public class SwitchSwapStatusCommand : IRequest<ResponseDto<string>>
    {

        public string UserId { get; set; }
        public string SwapId { get; set; }
        public SwapProceedingStatus Status { get; set; }
    }
    
}
