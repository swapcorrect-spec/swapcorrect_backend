using Swap_Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Enitities
{
    public class ReportMedia : BaseEntity
    {
        public string ReportId { get; set; }
        public UserReport Report { get; set; }
        public string MediaType { get; set; }
        public string Url { get; set; }
    }
}
