using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class UserListDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsOnline { get; set; }
        public string Status { get; set; }
        public int RatingScore { get; set; }
        public int SwapCompleted { get; set; }
        public string UserRole { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime? LastActive { get; set; }
    }
}
