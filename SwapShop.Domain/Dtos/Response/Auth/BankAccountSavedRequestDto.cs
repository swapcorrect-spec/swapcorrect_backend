using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Auth
{
    public class BankAccountSavedRequestDto
    {
        public   string BankCode { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
    }
}
