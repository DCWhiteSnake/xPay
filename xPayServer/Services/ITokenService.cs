using xPayServer.Models;
using System.Security.Claims;

namespace xPayServer.Services
{
    public interface ITokenService
    {
        double ExpiryInMinutes { get;set;}
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public Task<List<Claim>> GetClaims(ApplicationUser user);
    }
}
