using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.UserReviewRating
{
    public class ReviewMsgResp
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string ReviewerImg { get; set; }
        public string ReviewerName { get; set; }
        public int RateValue { get; set; }
    
        public DateTime DateCreated { get; set; }
    }
}
