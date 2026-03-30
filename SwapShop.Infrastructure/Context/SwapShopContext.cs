using Austistic.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Dtos.Response.FavListItems;
using SwapShop.Domain.Enitities;

namespace SwapShop.Infrastructure.Context
{
    public class SwapShopContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<FavListItem> FavListItems { get; set; }
        public DbSet<UserFavouritesItem> UserFavourites { get; set; }
        public DbSet<ConfirmEmailToken> ConfirmEmailTokens { get; set; }
        public DbSet<ForgetPasswordToken> ForgetPasswordTokens { get; set; }
        public DbSet<AdminNote> AdminNotes { get; set; }
        public DbSet<UserActivitylog> UserActivitylogs { get; set; }
        public DbSet<ListingItem> ListingItems { get; set; }
        public DbSet<ItemCategory> ItemCategorys { get; set; }
        public DbSet<OTP> OTP { get; set; }
        public DbSet<Item_Media> ItemMedia { get; set; }
        public DbSet<Saved_Item> Saved_Items { get; set; }
        public DbSet<User_Review_Rating> User_Review_Ratings { get; set; }
        public DbSet<UserRoom> UserRooms { get; set; }
        public DbSet<ReadMassageCount> ReadMassageCounts { get; set; }
        public DbSet<RoomMessages> RoomMessages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<SwappingProceeding> SwappingProceedings { get; set; }
        public DbSet<ReportMedia> ReportMedias { get; set; }
        public DbSet<UserReport> UserReport { get; set; }
        public DbSet<ReportAdminNote> ReportAdminNotes { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public SwapShopContext(DbContextOptions options) : base(options) { }
    }
}
