using SwapShop.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Request.ListingItem
{
    public class UpdateListingReview
    {
        public string ListingId { get; set; }
        public ListingReiviewStage review { get; set; }
    }
}
