using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;

namespace SwapShop.Application.Commands.Payment
{
    public class IntializePaymentForPaystackCommand :
         IRequest<ResponseDto<PaystackInitializeResponse>>
    {
        public string ProceedingId { get; set; }
        public string  RoomName { get; set; }
        public string UserId { get; set; }
        public bool IsChargeFee { get; set; }
    }
}
