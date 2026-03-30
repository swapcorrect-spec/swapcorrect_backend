using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.UserReviewRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Queries.UserReviewRating
{
    public class GetReviewByRaterQuery : IRequest<ResponseDto<UserReviewResponseDto>>
    {
        public string RaterId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
