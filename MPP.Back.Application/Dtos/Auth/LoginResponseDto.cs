namespace MPP.Back.Application.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationUtc { get; set; }
    }
}
