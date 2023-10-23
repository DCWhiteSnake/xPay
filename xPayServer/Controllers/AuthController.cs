using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using xPayServer.Models;
using Swashbuckle.AspNetCore.Annotations;
using xPayServer.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace xPayServer.Controllers;

/// <summary>
/// Provides functionality for account creation and login for admin to the /Admin/Auth route.
/// </summary>
[Route("api/Admin/Auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    /// <summary>
    /// configuartion setting for JWT
    /// </summary>
    private readonly IConfigurationSection _jwtSettings;

    private readonly ITokenService _tokenService;
    /// <summary>
    /// constructor <c>AdminAccountController</c> initializes an AdminAccount instance
    /// (<paramref name="userManager"/>,<paramref name="configuration"/>).
    /// </summary>
    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenService tokenService)
    {
        _userManager = userManager;
        _jwtSettings = configuration.GetSection("JwtSettings");
        _tokenService = tokenService;
    }
    /// <summary>
    /// Endpoint responsible for authenticating and login a user.
    /// Achievable through token generation 
    /// </summary>
    /// <param name="adminLogin">contains login details for an admin</param>
    /// <returns>JWT Token for the user, used for authorization</returns>
    [SwaggerOperation(Summary = "Login for an admin")]
    [HttpPost("Login")]
    public async Task<ActionResult<AuthenticatedResponseModel>> LoginModel([FromForm, SwaggerRequestBody("Account details payload", Required = true)] LoginModel adminLogin)
    {
        var user = await _userManager.FindByNameAsync(adminLogin.Username);
        if (user is null)
        {
            var error = new Dictionary<string, string> {
                { "error", $"User with username {adminLogin.Username} does not exist"}};
            return BadRequest(JsonConvert.SerializeObject(error));
        }

        if (await _userManager.IsInRoleAsync(user, "admin"))
        {
            if (await _userManager.CheckPasswordAsync(user, adminLogin.Password))
            {
                var refreshTokenExpiryInHours = 24;
                var claims = await _tokenService.GetClaims(user);
                var token = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddHours(refreshTokenExpiryInHours);
                await _userManager.UpdateAsync(user);
                return Ok(new AuthenticatedResponseModel() { Token = token, RefreshToken = refreshToken });
            }
            else
            {
                return Unauthorized("Username and password don't match");
            }
        }
        return Unauthorized("Invalid Authentication");
    }
    [SwaggerOperation(Summary = "Checks if the user's token is valid, returns Ok if it is")]
    [Authorize(Roles="admin")]
    [HttpGet("sayAutnHello")]
    public IActionResult SayHello(){
        var greeting = new Dictionary<string, string> {
            {"greeting", "hello"}
        };
        return Ok(JsonConvert.SerializeObject(greeting));
    }
    
    #region DeleteUsersAccount
    [Authorize(Roles = "admin")]
    [SwaggerOperation(Summary = "This allows the Admin to delete a customer user account")]
    [HttpDelete("DeleteCustomerAccount")]
    public async Task<IActionResult> DeleteCustomerAccount(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null) { return BadRequest($"User with username {username} does not exist"); }
        if (await _userManager.IsInRoleAsync(user, "Customer"))
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("Customer User Deleted");
            }
        }
        return BadRequest("Invalid Authentication");
    }

    [Authorize(Roles = "admin")]
    [SwaggerOperation(Summary = "Admin to delete a user account with this endpoint")]
    [HttpDelete("DeleteuserAccount")]
    public async Task<IActionResult> DeleteuserAccount(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null) { return BadRequest($"User with username {username} does not exist"); }
        if (await _userManager.IsInRoleAsync(user, "user"))
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("user User Deleted");
            }
        }
        return BadRequest("Invalid Authentication");
    }
    #endregion
}