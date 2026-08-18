using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.Enum;

namespace SwapShop.Application.Queries.Payment
{
    public class GetWithdrawalsQuery : IRequest<ResponseDto<PaginatedResult<WithdrawalResponseDto>>>
    {
        public string? UserId { get; set; }
        public WithdrawalStatus Status { get; set; } = WithdrawalStatus.All;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
