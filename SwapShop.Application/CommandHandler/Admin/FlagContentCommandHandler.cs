using MediatR;
using SwapShop.Application.Commands.Admin;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Admin
{
    public class FlagContentCommandHandler : IRequestHandler<FlagContentCommand, ResponseDto<string>>
    {
        private readonly IListItemService _listItemService;

        public FlagContentCommandHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }

        public async Task<ResponseDto<string>> Handle(FlagContentCommand request, CancellationToken cancellationToken)
        {
            return await _listItemService.FlagContent(request.ContentId, request.ContentType, request.IsFlagged);
        }
    }
}
