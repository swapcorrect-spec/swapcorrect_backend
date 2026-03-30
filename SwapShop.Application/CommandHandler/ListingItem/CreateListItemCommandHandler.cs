using AutoMapper;
using MediatR;
using SwapShop.Application.Commands.FavListItems;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.ListingItem
{
    public class CreateListItemCommandHandler : IRequestHandler<ListItemCommand, ResponseDto<string>>
    {
        private readonly IListItemService _listItemService;
        private readonly IMapper _mapper;

        public CreateListItemCommandHandler(IListItemService listItemService,IMapper mapper)
        {
            _listItemService = listItemService;
            _mapper = mapper;
        }

        public async Task<ResponseDto<string>> Handle(ListItemCommand request, CancellationToken cancellationToken)
        {
            var mapData = _mapper.Map<ListItemReq>(request);
            return await _listItemService.CreateItemRequest(mapData, request.UserId);
        }
    }
}
