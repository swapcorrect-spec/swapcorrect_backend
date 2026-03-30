using MediatR;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.ListingItem
{
    public class AdminReviewCommandHandler : IRequestHandler<AdminReviewCommand, ResponseDto<string>>
    {
        private readonly IListItemService _listItemService;
        public AdminReviewCommandHandler(IListItemService listItemService)
        {
            _listItemService = listItemService;
        }

        public async Task<ResponseDto<string>> Handle(AdminReviewCommand request, CancellationToken cancellationToken)
        {
            return await _listItemService.AdminReview(request.UserId, request.ListingId, request.review);
        }
    }
}
