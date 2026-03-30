using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;


namespace SwapShop.Domain.OtherService.Interface
{
    public interface IUserRatingService
    {
        Task<ResponseDto<User_Review_Rating>> AddReviewAsync(CreateUserReviewDto dto);
        Task<ResponseDto<string>> RemoveReviewAsync(string raterId, string userId);
        Task<ResponseDto<ReviewResp>> GetAllUserReview(string userId);
        
        Task<ResponseDto<UserReviewResponseDto>> GetReviewByRaterAsync(string raterId, string userId);
    }
}
