namespace SwapShop.Domain.Dtos.Response.Chat
{
    public class RoomMessageResp
    {
        public string message { get; set; }
        public string DateTime { get; set; }
        public string status { get; set; }
        public string messageType { get; set; }
        public string senderImgUrl { get; set; }
        public string senderId { get; set; }
        public bool isMe { get; set; }
    }

}
