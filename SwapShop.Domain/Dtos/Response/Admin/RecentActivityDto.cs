using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class RecentActivityDto
    {
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string TimeAgo { get; set; }
    }
}
