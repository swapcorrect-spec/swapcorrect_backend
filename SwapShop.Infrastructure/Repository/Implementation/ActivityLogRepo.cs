using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Repository.Interface;

namespace SwapShop.Infrastructure.Repository.Implementation
{
    public class ActivityLogRepo : IActivityLogRepo
    {
        private readonly ISwapShopGenericRepo<UserActivitylog> _userActivitylogRepo;
        private readonly ILogger<ActivityLogRepo> _logger;

        public ActivityLogRepo(ISwapShopGenericRepo<UserActivitylog> userActivitylogRepo, 
            ILogger<ActivityLogRepo> logger)
        {
            _userActivitylogRepo = userActivitylogRepo;
            _logger = logger;
        }
        public async Task< ResponseDto<bool>> AddActivitylog(string userid, string activityType, string activityDescription)
        {
            var result = new ResponseDto<bool>();
            try
            {
                var add = await _userActivitylogRepo.Add(new UserActivitylog()
                {
                    UserId = userid,
                    ActivitiesDescription = activityDescription,
                    ActivitiesType = activityType,
                });
                await _userActivitylogRepo.SaveChanges();
                result.StatusCode = 200;
                result.DisplayMessage = "Success";
                result.Result = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, JsonConvert.SerializeObject(ex));
                result.ErrorMessages = new List<string>() { "Error in logging activities" };
                result.StatusCode = 501;
                result.DisplayMessage = "Error";
                return result;
            }
        }
    }
}
