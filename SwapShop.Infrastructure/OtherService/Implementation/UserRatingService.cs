using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;
using SwapShop.Infrastructure.Context;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class UserRatingService : IUserRatingService
    {
        private readonly SwapShopContext _context;
        private readonly ISwapShopGenericRepo<User_Review_Rating> _userReviewRatingRepo;
        private readonly ILogger<UserRatingService> _logger;
        public UserRatingService(SwapShopContext context,
            ISwapShopGenericRepo<User_Review_Rating> userReviewRatingRepo,
            ILogger<UserRatingService> logger)
        {
            _context = context;
            _userReviewRatingRepo = userReviewRatingRepo;
            _logger = logger;
        }

        public async Task<ResponseDto<User_Review_Rating>> AddReviewAsync(CreateUserReviewDto dto)
        {

            if (dto.RateScore < 1 || dto.RateScore > 5)
            {
                return new ResponseDto<User_Review_Rating>
                {
                    StatusCode = 400,
                    DisplayMessage = "Rating must be between 1 and 5.",
                    ErrorMessages = new List<string> { "Invalid rating value." }
                };
            }

            var existingReview = await _context.User_Review_Ratings
                .FirstOrDefaultAsync(r => r.RaterId == dto.RaterId && r.UserId == dto.UserId);

            if (existingReview != null)
            {
                return new ResponseDto<User_Review_Rating>
                {
                    StatusCode = 409,
                    DisplayMessage = "You have already rated this user.",
                    ErrorMessages = new List<string> { "Duplicate review not allowed." }
                };
            }

            var review = new User_Review_Rating
            {
                RaterId = dto.RaterId,
                UserId = dto.UserId,
                RateScore = dto.RateScore,
                RatingDescription = dto.Description
            };

            _context.User_Review_Ratings.Add(review);
            await _context.SaveChangesAsync();

            return new ResponseDto<User_Review_Rating>
            {
                StatusCode = 201,
                DisplayMessage = "Review added successfully.",
                Result = review
            };
        }
        public async Task<ResponseDto<ReviewResp>> GetAllUserReview(string userId)
        {

            var response = new ResponseDto<ReviewResp>();
            try
            {
                var getAllUserReview = await _userReviewRatingRepo
                    .GetQueryable().Include(u => u.Rater).Where(r => r.UserId == userId).ToListAsync();
                if (!getAllUserReview.Any())
                {
                    response.StatusCode = 200;
                    response.DisplayMessage = "Success";
                    return response;
                }
                var totalReviewCount = getAllUserReview.Count();
                var reviewAvg = getAllUserReview.Sum(u => u.RateScore) / totalReviewCount;
                var total1RatingCount = getAllUserReview.Count(u => u.RateScore == 1);
                var total2RatingCount = getAllUserReview.Count(u => u.RateScore == 2);
                var total3RatingCount = getAllUserReview.Count(u => u.RateScore == 3);
                var total4RatingCount = getAllUserReview.Count(u => u.RateScore == 4);
                var total5RatingCount = getAllUserReview.Count(u => u.RateScore == 5);
                var allMessage = getAllUserReview.Select(u => new ReviewMsgResp
                {
                    DateCreated = u.Created,
                    Message = u.RatingDescription,
                    RateValue = u.RateScore,
                    ReviewerImg = u.Rater.ProfilePicture,
                    ReviewerName = u.Rater.FirstName + " " + u.Rater.LastName
                }).ToList();
                var result = new ReviewResp()
                {
                    TotalAvgReview = reviewAvg,
                    ReviewMsgList = allMessage,
                    Total1ReviewCount = total1RatingCount,
                    Total2ReviewCount = total2RatingCount,
                    Total3ReviewCount = total3RatingCount,
                    Total4ReviewCount = total4RatingCount,
                    Total5ReviewCount = total5RatingCount,
                    TotalReviewCount = totalReviewCount

                };
                response.Result = result;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllUserReview");
                response.ErrorMessages = new List<string> { "Error in Get All User Review" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<UserReviewResponseDto>> GetReviewByRaterAsync(string raterId, string userId)
        {
            var review = await _context.User_Review_Ratings
                .Include(r => r.Rater)
                .FirstOrDefaultAsync(r => r.RaterId == raterId && r.UserId == userId);

            if (review == null)
            {
                return new ResponseDto<UserReviewResponseDto>
                {
                    StatusCode = 404,
                    DisplayMessage = "Review not found.",
                    ErrorMessages = new List<string> { "No review by this user." }
                };
            }

            var dto = new UserReviewResponseDto
            {
                Id = review.Id,
                RaterId = raterId,
                UserId = userId,
                RateScore = review.RateScore,
                Created = review.Created,
                Description = review.RatingDescription
            };

            return new ResponseDto<UserReviewResponseDto>
            {
                StatusCode = 200,
                DisplayMessage = "Review fetched successfully.",
                Result = dto
            };
        }

    
        public async Task<ResponseDto<string>> RemoveReviewAsync(string raterId, string userId)
        {
            var review = await _context.User_Review_Ratings
               .FirstOrDefaultAsync(r => r.RaterId == raterId && r.UserId == userId);

            if (review == null)
            {
                return new ResponseDto<string>
                {
                    StatusCode = 404,
                    DisplayMessage = "Review not found.",
                    ErrorMessages = new List<string> { "Review does not exist." }
                };
            }

            _context.User_Review_Ratings.Remove(review);
            await _context.SaveChangesAsync();

            return new ResponseDto<string>
            {
                StatusCode = 200,
                DisplayMessage = "Review removed successfully.",
                Result = "Deleted"
            };
        }
    }
}
