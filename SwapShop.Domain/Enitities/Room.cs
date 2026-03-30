using Austistic.Core.Entities;

namespace SwapShop.Domain.Enitities
{
    public class Room : BaseEntity
    {
        public string UserRoomId { get; set; }
        public string RoomName { get; set; }
        public UserRoom UserRoom { get; set; }
        public List<RoomMessages> Messages { get; set; }
    }
}
