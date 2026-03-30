using MediatR;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Queries.ListingItem
{
    public class SearchPaginatedListingSwapQuery : IRequest<ResponseDto<PaginatedResult<SwapProceedingResp>>>
    {
      
        public string? ListingUserId { get; set; }
        public string? SearhParam { get; set; }
        public SwapListingEnumStatus SwapListingStatus { get; set; }
        public ListingDateFilter ListingDate { get; set; }
        public int PageNumber { get; set; }
        public int PerPageSize { get; set; }
    }
}
