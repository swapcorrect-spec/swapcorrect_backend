using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.Enum;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IPaymentService
    {
        Task<ResponseDto<PaystackInitializeResponse>> IntializePaymentForPaystack(string proceedingId, string userid, bool isChargeFee, string roomName);
        Task<ResponseDto<string>> ConfirmPaystackpayment(string reference, string userid);
        Task<ResponseDto<List<PaystackBank>>> BankDetails();
        Task<ResponseDto<PaginatedResult<TransactionDto>>> GetAllTransactions(string? userId, string? searchParam, TransactionDateFilter dateFilter, int pageNumber, int pageSize);
        Task<ResponseDto<TransactionStatsDto>> GetTransactionStats();
        Task<ResponseDto<string>> SubmitWithdrawal(string userId, string swapId);
        Task<ResponseDto<string>> TreatWithdrawal(string withdrawalId, string? adminNote);
        Task<ResponseDto<string>> CompleteAdvanceSwap(string userId, string swapId);
        Task<ResponseDto<PaginatedResult<WithdrawalResponseDto>>> GetWithdrawals(string? userId, WithdrawalStatus status, int pageNumber, int pageSize);
    }
}
