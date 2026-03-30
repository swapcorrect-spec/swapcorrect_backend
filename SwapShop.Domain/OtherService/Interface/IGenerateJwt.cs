using Swap_Shop.Domain.Entities;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IGenerateJwt
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
