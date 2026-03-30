using MediatR;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.ListingItem
{
    public class CloseSwapCommandHandler : IRequestHandler<CloseSwapCommand, ResponseDto<string>>
    {
        private readonly IListItemService _listItemService;

        public CloseSwapCommandHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }

        public async Task<ResponseDto<string>> Handle(CloseSwapCommand request, CancellationToken cancellationToken)
        {
            return await _listItemService.CloseSwap(request.UserId, request.SwapId);
        }
    }
}
