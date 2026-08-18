namespace SwapShop.Domain.Dtos.Response
{
    public class LoginResultDto
    {
        public string Jwt { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public IList<string>? UserRole { get; set; }
    }
}
