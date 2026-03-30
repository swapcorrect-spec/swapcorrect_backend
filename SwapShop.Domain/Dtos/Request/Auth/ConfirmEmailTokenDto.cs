namespace SwapShop.Domain.Dtos.Request.Auth
{
    public class ConfirmEmailTokenDto
    {
        public string email { get; set; }
        public int token { get; set; }
    }
}
