using SwapShop.Domain.Dtos.Response.Payment;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IPaystackService
    {
        Task<PaystackInitializeResponse> InitializeTransactionAsync(int amount, string email, string reference, string callbackurl);
        Task<bool> VerifyTransactionAsync(string reference);
        Task<PaystackBankResponse> GetAllBank();
    }
}
