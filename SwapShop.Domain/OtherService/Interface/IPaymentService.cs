using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IPaymentService
    {
        Task<ResponseDto<PaystackInitializeResponse>> IntializePaymentForPaystack(string proceedingId, string userid, bool isChargeFee, string roomName);
        Task<ResponseDto<string>> ConfirmPaystackpayment(string reference, string userid);
        Task<ResponseDto<List<PaystackBank>>> BankDetails();
    }
}
