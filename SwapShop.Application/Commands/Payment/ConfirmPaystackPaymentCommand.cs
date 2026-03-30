using MediatR;
using SwapShop.Domain.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Commands.Payment
{
    public class ConfirmPaystackPaymentCommand :
         IRequest<ResponseDto<string>>
    {
        public string Reference { get; set; }
        public string UserId { get; set; }
    }
}
