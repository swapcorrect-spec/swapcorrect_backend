namespace SwapShop.Domain.Dtos.Response.Notification
{
    public class NotiResp
    {
        public bool IsEmailActivated { get; set; }
        public bool IsSmsActivated { get; set; }
        public bool IsPushActivated { get; set; }
    }
}
