using MediatR;
using SwapShop.Application.Commands.UserReviewRating;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.UserReviewRating
{
    public class RemoveReviewCommandHandler : IRequestHandler<RemoveReviewCommand, ResponseDto<string>>
    {
        private readonly IUserRatingService _reviewService;

        public RemoveReviewCommandHandler(IUserRatingService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<ResponseDto<string>> Handle(RemoveReviewCommand request, CancellationToken cancellationToken)
        {
            var result = await _reviewService.RemoveReviewAsync(request.raterId, request.userId);

            if (result == null || result.Result == null)
            {
                return new ResponseDto<string>
                {
                    StatusCode = 404,
                    DisplayMessage = "Unable to remove review",
                    ErrorMessages = new List<string> { "Review not found or already removed." },
                    Result = null
                };
            }

            return new ResponseDto<string>
            {
                StatusCode = result.StatusCode,
                DisplayMessage = result.DisplayMessage,
                Result = result.Result
            };
        }
    }
}
