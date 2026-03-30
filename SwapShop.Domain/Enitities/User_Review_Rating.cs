using SwapShop.Domain.Enitities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swap_Shop.Domain.Entities
{
    public class User_Review_Rating : BaseEntity
    {
        public string RaterId { get; set; }
        [ForeignKey("RaterId")]
        public ApplicationUser Rater { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
                                             
        public int RateScore { get; set; }
        public string  RatingDescription { get; set; }
    }
}
