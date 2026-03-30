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
    }
}
