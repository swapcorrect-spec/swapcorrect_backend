using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Enitities
{
    public class ReportAdminNote : BaseEntity
    {
        public string Note { get; set; }
        public string ReportId { get; set; }
        public UserReport Report { get; set; }
    }
}
