using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Exata.Helpers.Interfaces;

namespace Exata.Helpers;

public class Token : IToken
{
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IVariaveisAmbiente varAmbiente)
    {
        var key = varAmbiente.SecretKey;
        var privateKey = Encoding.UTF8.GetBytes(key);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
                                 SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(varAmbiente.TokenValidityInMinutes)),
            Audience = varAmbiente.ValidAudience,
            Issuer = varAmbiente.ValidIssuer,
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return token;
    }

    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(secureRandomBytes);
        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IVariaveisAmbiente varAmbiente)
    {        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                                  Encoding.UTF8.GetBytes(varAmbiente.SecretKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                                                   out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                         !jwtSecurityToken.Header.Alg.Equals(
                         SecurityAlgorithms.HmacSha256,
                         StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
