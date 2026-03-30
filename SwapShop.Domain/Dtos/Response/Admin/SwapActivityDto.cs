using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class SwapActivityDto
    {
        public string Id { get; set; }
        public string OwnerName { get; set; }
        public string SwapperName { get; set; }
        public string OwnerItem { get; set; }
        public string SwapperItem { get; set; }
        public string Status { get; set; }
        public DateTime InitiatedOn { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
