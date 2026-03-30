

using System.ComponentModel.DataAnnotations;

namespace SwapShop.Domain.Dtos.Response.FavListItems
{
    public class UserFavouritesItem
    {
        [Key]
        public int FavId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ListId { get; set; } = string.Empty;
    }
}
