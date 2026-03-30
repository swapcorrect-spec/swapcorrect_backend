using MediatR;
using SwapShop.Domain.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Commands.ListItem
{
    public class RemoveSingleListingCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
        public string ListingId { get; set; }
    }
}
