using MediatR;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Commands.ListItem
{
    public class UpdateListingCommand : IRequest<ResponseDto<string>>
    {
        public UpdateListingReq Req { get; set; }
        public string UserId { get; set; }
    }
}
