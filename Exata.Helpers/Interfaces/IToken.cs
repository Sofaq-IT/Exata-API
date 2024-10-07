using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Exata.Helpers.Interfaces;

public interface IToken
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IVariaveisAmbiente varAmbiente);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IVariaveisAmbiente varAmbiente);
}
