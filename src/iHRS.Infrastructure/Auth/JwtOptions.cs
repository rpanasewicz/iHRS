namespace iHRS.Infrastructure.Auth
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string IssuerSigningKey { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}