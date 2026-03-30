using MediatR;
using SwapShop.Application.Queries.UserReviewRating;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.QueryHandler.UserReviewRating
{
    public class GetReviewByRaterQueryHandler : IRequestHandler<GetReviewByRaterQuery, ResponseDto<UserReviewResponseDto>>
    {
        private readonly IUserRatingService _userRatingService;
        public GetReviewByRaterQueryHandler(IUserRatingService userRatingService)
        {
            _userRatingService = userRatingService;
        }
        public async Task<ResponseDto<UserReviewResponseDto>> Handle(GetReviewByRaterQuery request, CancellationToken cancellationToken)
        {
            return await _userRatingService.GetReviewByRaterAsync(request.RaterId, request.UserId);
        }
    }
}
