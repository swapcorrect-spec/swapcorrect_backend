using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class PaginationFilterDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; } // optional search across name/email
        public string? SortBy { get; set; } // optional: "name", "rating", "swaps", "date"
        public bool SortDesc { get; set; } = true;
    }
}
