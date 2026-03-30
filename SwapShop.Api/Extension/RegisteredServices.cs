using ProjectX.Api.MappingProfile;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;
using SwapShop.Infrastructure.OtherService.Implementation;
using SwapShop.Infrastructure.Repository.Implementation;

namespace SwapSwap.Api.MappingProfile
{
    public static class RegisteredServices
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountRepo, AccountRepo>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped(typeof(ISwapShopGenericRepo<>), typeof(SwapShopGenericRepo<>));
            services.AddScoped<IGenerateJwt, GenerateJwt>();
            services.AddScoped<IActivityLogRepo, ActivityLogRepo>();
            services.AddScoped<IEmailServices, EmailService>();
            services.AddScoped<IFavListItemService, FavListItemService>();
            services.AddScoped<IUserRatingService, UserRatingService>();
            services.AddSingleton<IEncryptionService, EncryptionService>();
            
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IListItemService, ListItemService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IPaystackService, PaystackService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IHelper, Helper>();
            services.AddAutoMapper(typeof(ProjectProfile));
            services.AddHttpClient();
            services.AddSignalR();
        }
    }
}
