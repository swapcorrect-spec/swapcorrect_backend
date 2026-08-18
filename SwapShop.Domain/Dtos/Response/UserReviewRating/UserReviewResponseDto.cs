using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.UserReviewRating
{
    public class UserReviewResponseDto
    {
        public string Id { get; set; }
        public string RaterId { get; set; }
        public string UserId { get; set; }
        public int RateScore { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public ReviewUserDto Rater { get; set; }
        public ReviewUserDto User { get; set; }
    }

    public class ReviewUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool IsOnline { get; set; }
        public string? LastSeen { get; set; }
    }
}
