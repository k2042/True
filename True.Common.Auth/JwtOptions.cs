namespace True.Common.Auth
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpirationInMinutes { get; set; }
        public string SecretKey { get; set; } = null!;
    }
}
