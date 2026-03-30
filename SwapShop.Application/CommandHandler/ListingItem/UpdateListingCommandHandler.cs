using MediatR;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.CommandHandler.ListingItem
{
    public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand, ResponseDto<string>>
    {
        private readonly IListItemService _listItemService;
        public UpdateListingCommandHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<string>> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
        {
            return await _listItemService.UpdateListingItemRequest(request.Req, request.UserId);
        }
    }
}
