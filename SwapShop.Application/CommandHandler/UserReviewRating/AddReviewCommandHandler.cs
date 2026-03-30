using MediatR;
using SwapShop.Application.Commands.UserReviewRating;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.UserReviewRating
{
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, ResponseDto<string>>
    {
        private readonly IUserRatingService _review;
        public AddReviewCommandHandler(IUserRatingService review)
        {
            _review = review;
        }

        public async Task<ResponseDto<string>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<string>
            {
                ErrorMessages = new List<string>()

            };
            var result = await _review.AddReviewAsync(request.dto);
            if (result == null || result.Result == null)
            {
                response.StatusCode = 400;
                response.DisplayMessage = "Unable to add review";
                response.Result = null;
                response.ErrorMessages.Add("Review service returned null");
            }
            else
            {
                response.StatusCode = 201;
                response.DisplayMessage = "Review added successfully";
                response.Result = result.Result.Id;
            }
            return response;
        }
    }
}
