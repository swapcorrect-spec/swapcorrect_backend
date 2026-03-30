using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class DashboardCardDto
    {
        public int Count { get; set; }
        public double PercentageChange { get; set; } // +12% or -12%
        public bool IsIncrease { get; set; }         // true = increase, false = decrease
    }
}
