using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xPayServer.Data;
using xPayServer.Models;
using xPayServer.Services;

namespace xPayServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
        private readonly ApplicationIdentityDbContext _ApplicationIdentityDbContext;
        private readonly ITokenService _tokenService;

        public TokenController(ApplicationIdentityDbContext ApplicationIdentityDbContext, ITokenService tokenService)
        {
                this._ApplicationIdentityDbContext = ApplicationIdentityDbContext ?? throw new ArgumentNullException(nameof(ApplicationIdentityDbContext));
                this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenApiModel tokenApiModel)
        {
                if (tokenApiModel is null)
                {
                        var errorObject = new Dictionary<string, string> {
                        {"error", "Invalid client request, refresh token cannot be null"}
                };
                        return BadRequest(JsonConvert.SerializeObject(errorObject));
                }
                string accessToken = tokenApiModel.Token;
                string refreshToken = tokenApiModel.RefreshToken;

                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
                var username = principal.Identity.Name; //this is mapped to the Name claim by default

                var user = _ApplicationIdentityDbContext.Users.SingleOrDefault(u => u.UserName == username);

                if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                        var errorObject = new Dictionary<string, string> {
                        {"error", "Invalid token"}};
                        return BadRequest(errorObject);
                }
                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                _ApplicationIdentityDbContext.SaveChanges();

                return Ok(new AuthenticatedResponseModel()
                {
                        Token = newAccessToken,
                        RefreshToken = newRefreshToken
                });
        }

        [HttpPost, Authorize(Roles = "admin")]
        [Route("revoke")]
        public IActionResult Revoke()
        {
                var username = User.Identity.Name;

                var user = _ApplicationIdentityDbContext.Users.SingleOrDefault(u => u.UserName == username);
                if (user == null) return BadRequest();

                user.RefreshToken = null;

                _ApplicationIdentityDbContext.SaveChanges();

                return NoContent();
        }
}