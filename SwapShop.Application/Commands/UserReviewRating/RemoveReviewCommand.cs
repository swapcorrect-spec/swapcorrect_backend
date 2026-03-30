using MediatR;
using SwapShop.Domain.Dtos.Response;


namespace SwapShop.Application.Commands.UserReviewRating
{
    public class RemoveReviewCommand : IRequest<ResponseDto<string>>
    {
        public string raterId { get; set; }
        public string userId { get; set; }
    }
}
