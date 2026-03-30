using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.UserReviewRating
{
    public class ReviewResp
    {
        public int TotalAvgReview {  get; set; }
        public int TotalReviewCount {  get; set; }
        public int Total1ReviewCount {  get; set; }
        public int Total2ReviewCount {  get; set; }
        public int Total3ReviewCount {  get; set; }
        public int Total4ReviewCount {  get; set; }
        public int Total5ReviewCount {  get; set; }
        public List<ReviewMsgResp> ReviewMsgList { get; set; }
        
    }
}
