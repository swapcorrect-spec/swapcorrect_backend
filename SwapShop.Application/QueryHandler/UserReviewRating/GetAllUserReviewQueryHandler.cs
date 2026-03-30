using MediatR;
using SwapShop.Application.Queries.UserReviewRating;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;
using SwapShop.Domain.OtherService.Interface;


namespace SwapShop.Application.QueryHandler.UserReviewRating
{
    public class GetAllUserReviewQueryHandler : IRequestHandler<GetAllUserReviewQuery, ResponseDto<ReviewResp>>
    {
        private readonly IUserRatingService _userRatingService;
        public GetAllUserReviewQueryHandler(IUserRatingService userRatingService)
        {
            _userRatingService = userRatingService;
        }
        public async Task<ResponseDto<ReviewResp>> Handle(GetAllUserReviewQuery request, CancellationToken cancellationToken)
        {
            return await _userRatingService.GetAllUserReview(request.UserId);
        }
    }
}
