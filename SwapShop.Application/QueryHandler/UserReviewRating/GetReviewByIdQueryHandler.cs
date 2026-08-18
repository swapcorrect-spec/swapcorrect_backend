using MediatR;
using SwapShop.Application.Queries.UserReviewRating;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.UserReviewRating
{
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ResponseDto<UserReviewResponseDto>>
    {
        private readonly IUserRatingService _userRatingService;

        public GetReviewByIdQueryHandler(IUserRatingService userRatingService)
        {
            _userRatingService = userRatingService;
        }

        public async Task<ResponseDto<UserReviewResponseDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userRatingService.GetReviewByIdAsync(request.ReviewId);
        }
    }
}
