using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SwapShop.Infrastructure.Context;


namespace SwapShop.Api.ResponsHandler
{
    public class Seeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {

            // Get db context
            var dbContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<SwapShopContext>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            if (!dbContext.Roles.Any())
            {
                await dbContext.Database.EnsureCreatedAsync();
                var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                List<string> roles = new() { "Admin", "Swapper","Visitor" };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
            await dbContext.SaveChangesAsync();
        }


    }
}
