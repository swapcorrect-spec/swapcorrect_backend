using SwapShop.Domain.Dtos.Request.Mailing;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IEmailServices
    {
        void SendEmail(Message message);
    }
}
