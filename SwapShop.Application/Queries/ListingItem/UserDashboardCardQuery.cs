using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.ListingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Queries.ListingItem
{
    public class UserDashboardCardQuery : IRequest<ResponseDto<UserDashboardCard>>
    {
        public string UserId { get; set; }
    }
}
