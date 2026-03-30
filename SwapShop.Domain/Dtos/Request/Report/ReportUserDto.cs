using SwapShop.Domain.Dtos.Request.ListingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Request.Report
{
    public class ReportUserDto
    {
        public string ReportedUserId { get; set; }
        public string Description { get; set; }
        public List<ListItemMedia> EvidenceMediaFiles { get; set; }
    }
}
