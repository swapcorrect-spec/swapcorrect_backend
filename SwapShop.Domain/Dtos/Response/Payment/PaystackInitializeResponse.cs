using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Payment
{
    public class PaystackInitializeResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public PaystackInitializeData Data { get; set; }
    }

    public class PaystackInitializeData
    {
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
    }

}
