using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Domain.Repository.Interface
{
    public interface IActivityLogRepo
    {
        Task<ResponseDto<bool>> AddActivitylog(string userid, string activityType, string activityDescription);
    }
}
