using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace True.Common.Auth
{
    public static class TokenHandling
    {
        public static JwtOptions? JwtOptionsInstance;

        public static string IssueTokenFor(string userName, IEnumerable<Claim>? additionalClaims = null)
        {
            if (JwtOptionsInstance == null)
            {
                throw new NullReferenceException("JWT options are not initialized");
            }

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtOptionsInstance.SecretKey)
            );

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new (JwtRegisteredClaimNames.Sub, userName)
            ];

            if (additionalClaims != null)
            {
                claims.AddRange(additionalClaims);
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(JwtOptionsInstance.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = JwtOptionsInstance.Issuer,
                Audience = JwtOptionsInstance.Audience,
            };

            var handler = new JsonWebTokenHandler();

            return handler.CreateToken(descriptor);
        }
    }
}
