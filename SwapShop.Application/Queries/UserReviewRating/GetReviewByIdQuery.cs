using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;

namespace SwapShop.Application.Queries.UserReviewRating
{
    public class GetReviewByIdQuery : IRequest<ResponseDto<UserReviewResponseDto>>
    {
        public string ReviewId { get; set; } = string.Empty;
    }
}
