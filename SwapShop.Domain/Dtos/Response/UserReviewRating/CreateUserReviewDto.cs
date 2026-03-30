using System.ComponentModel.DataAnnotations;

namespace SwapShop.Domain.Dtos.Response.UserReviewRating
{
    public class CreateUserReviewDto
    {
        [Required]
        public string RaterId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string UserId { get; set; }
        [Range (1, 5)]
        public int RateScore { get; set; }           
    }
}
