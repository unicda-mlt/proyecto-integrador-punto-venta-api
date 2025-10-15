using Domain.Authentication;
using Domain.Environment;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Authentication
{
    public class AuthenticationService(IOptions<TokenSetting> tokenSetting) : IAuthenticationService
    {
        private readonly TokenSetting _tokenSetting = tokenSetting.Value;

        public string GenerateUserToken(Guid usuarioId, int rolId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSetting.UserScheme.Key));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new Claim(ClaimTypes.Role, rolId.ToString()),
            };

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _tokenSetting.Issuer,
                audience: _tokenSetting.UserScheme.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenSetting.UserScheme.ExpiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenUserInfo? GetTokenUserInfo(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSetting.UserScheme.Key));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _tokenSetting.Issuer,
                ValidAudience = _tokenSetting.UserScheme.Audience,
                IssuerSigningKey = key,
                ValidateLifetime = true
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                var usuarioId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var rolId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (usuarioId == null || rolId == null)
                {
                    return null;
                }

                return new()
                {
                    UsuarioId = Guid.Parse(usuarioId),
                    RolId = int.Parse(rolId)
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
