using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Report
{
    public class UserReportPaginatedDto
    {
        public string ReportId{ get; set; }
        public string ReporterId{ get; set; }
        public string ReporterName { get; set; }
        public string ReporterImg { get; set; }
        public string ReportedPersonId { get; set; }
        public string ReportedPersonName { get; set; }
        public string ReportedPersonImg { get; set; }
        public string ReportType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
    }
}
