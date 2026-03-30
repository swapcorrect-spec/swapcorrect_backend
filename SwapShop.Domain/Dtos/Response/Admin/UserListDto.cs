using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class UserListDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public int RatingScore { get; set; }
        public string Status { get; set; }
        public int SwapCompleted { get; set; }
        public DateTime DateJoined { get; set; }
        public string UserRole { get; set; }
        public DateTime? LastActive { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
