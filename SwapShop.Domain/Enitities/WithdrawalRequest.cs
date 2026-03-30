using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Enitities
{
    public class WithdrawalRequest
    {
        public string SwapId { get; set; }
        public string Amount { get; set; }
        public SwappingProceeding Swap { get; set; }
    }
}
