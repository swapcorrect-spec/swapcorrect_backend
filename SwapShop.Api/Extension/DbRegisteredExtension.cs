using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swap_Shop.Domain.Entities;
using SwapShop.Infrastructure.Context;


namespace SwapSwap.Api.MappingProfile
{
    public static class DbRegisteredExtension
    {
        public static void ConfigureDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<SwapShopContext>()
                    .AddDefaultTokenProviders();

            services.AddDbContext<SwapShopContext>(dbContextOptions =>
            {
                var connectionString = configuration.GetConnectionString("ProdDB");
                var maxRetryCount = 3;
                var maxRetryDelay = TimeSpan.FromSeconds(10);

                dbContextOptions.UseNpgsql(connectionString, options =>
                {
                    options.EnableRetryOnFailure(maxRetryCount, maxRetryDelay, null);
                });
            });
        }
    }
}
