using MediatR;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Queries.ListingItem
{
    public class SingleSwapProceedingQuery : IRequest<ResponseDto<SwapProceedingResp>>
    {
        public string SwapProceedId { get; set; }
    }
}
