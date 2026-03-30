using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;


namespace SwapShop.Application.Commands.UserReviewRating
{
    public class AddReviewCommand : IRequest<ResponseDto<string>>
    {
        public CreateUserReviewDto dto { get; set; }
    }
}
