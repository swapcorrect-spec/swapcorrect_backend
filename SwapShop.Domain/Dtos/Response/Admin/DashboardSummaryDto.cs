using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class DashboardSummaryDto
    {
        public DashboardCardDto Swapper { get; set; }
        public DashboardCardDto Visitor { get; set; }
        public DashboardCardDto ActiveUsers { get; set; }
        public DashboardCardDto RegisteredUsers { get; set; }
        public DashboardCardDto CompletedSwaps { get; set; }
        public DashboardCardDto ActiveSwaps { get; set; }
    }
}
