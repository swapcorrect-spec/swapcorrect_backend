using MediatR;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.ListingItem
{
    public class CloseListingCommandHandler : IRequestHandler<CloseListingCommand, ResponseDto<string>>
    {
        private readonly IListItemService _listItemService;

        public CloseListingCommandHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }

        public Task<ResponseDto<string>> Handle(CloseListingCommand request, CancellationToken cancellationToken)
            => _listItemService.CloseListing(request.UserId, request.ListingId);
    }
}