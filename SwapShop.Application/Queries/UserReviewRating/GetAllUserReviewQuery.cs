using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;


namespace SwapShop.Application.Queries.UserReviewRating
{
    public class GetAllUserReviewQuery : IRequest<ResponseDto<ReviewResp>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
