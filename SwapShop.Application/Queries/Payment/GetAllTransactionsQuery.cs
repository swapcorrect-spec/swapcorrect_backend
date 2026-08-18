using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.Enum;

namespace SwapShop.Application.Queries.Payment
{
    public class GetAllTransactionsQuery : IRequest<ResponseDto<PaginatedResult<TransactionDto>>>
    {
        public string? UserId { get; set; }
        public string? SearchParam { get; set; }
        public TransactionDateFilter DateFilter { get; set; } = TransactionDateFilter.All;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
