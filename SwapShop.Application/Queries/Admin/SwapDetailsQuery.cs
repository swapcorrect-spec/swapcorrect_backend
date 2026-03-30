using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Queries.Admin
{
    public class SwapDetailsQuery : IRequest<ResponseDto<SwapDetailsDto>>
    {
        public string SwapId { get; set; }
    }
}
