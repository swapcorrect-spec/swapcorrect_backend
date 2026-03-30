using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Request.Payment
{
    public class InitializePayment
    {
        public string ProceedingId { get; set; }
        public string RoomName { get; set; }
   
        public bool IsChargeFee { get; set; }
    }
}
