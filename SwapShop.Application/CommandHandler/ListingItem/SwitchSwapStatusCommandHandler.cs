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
    public class SwitchSwapStatusCommandHandler : IRequestHandler<SwitchSwapStatusCommand, ResponseDto<string>>
    {
        private readonly IListItemService _listItemService;
        public SwitchSwapStatusCommandHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }
        public async Task<ResponseDto<string>> Handle(SwitchSwapStatusCommand request, CancellationToken cancellationToken)
        {
            return await _listItemService.SwitchSwapStatus(request.UserId, request.SwapId, request.Status);
        }
    }
}
