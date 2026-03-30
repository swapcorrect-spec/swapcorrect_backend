using AutoMapper;
using MediatR;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.ListingItem
{
    public class StartSwapCommandHandler : IRequestHandler<StartSwapCommand, ResponseDto<string>>
    {
        private readonly IListItemService _listItemService;
        private readonly IMapper _mapper;

        public StartSwapCommandHandler(IListItemService listItemService, IMapper mapper)
        {
            _listItemService = listItemService;
            _mapper = mapper;
        }

        public async Task<ResponseDto<string>> Handle(StartSwapCommand request, CancellationToken cancellationToken)
        {
            return await _listItemService.StartSwap(request.UserId, request.ListingId);
        }
    }
}
