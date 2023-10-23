using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using xPayServer.Models;
using xPayServer.Models.Dtos;
using xPayServer.Services;

namespace xPayServer.Controllers;
/// <summary>
/// Provides functionality for accounts manipulation for admin to the /Admin/Accounts route.
/// </summary>
[Route("api/Admin/Accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAccountsRepository _accountsRepository;
        public AccountsController(UserManager<ApplicationUser> userManager, IMapper mapper
        , IAccountsRepository accountsRepository)
        {
                _userManager = userManager;
                _mapper = mapper;
                _accountsRepository = accountsRepository;
        }

        /// <summary>
        /// Endpoint responsible for getting users
        /// </summary>
        /// <returns> A list of users </returns>
        [SwaggerOperation(Summary = "Return Users")]
        [HttpGet("users")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<ApplicationUserForDisplayDto>> GetAllUsers()
        {
                var users = _accountsRepository.GetAllUsers();
                var usersToDisplay = _mapper.Map<IEnumerable<ApplicationUserForDisplayDto>>(users);
                return Ok(usersToDisplay);
        }
        /// <summary>
        /// Endpoint responsible for getting flagged users
        /// </summary>
        /// <returns> A list of flagged users </returns>
        [SwaggerOperation(Summary = "Return flagged users")]
        [HttpGet("flagged")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<ApplicationUserForDisplayDto>> GetFlaggedUsers()
        {
                var users = _accountsRepository.GetFlaggedUsers();
                return Ok(users);
        }
        /// <summary>
        /// Endpoint responsible for getting locked out users
        /// </summary>
        /// <returns> A list of locked out users </returns>
        [SwaggerOperation(Summary = "Return locked out users")]
        [HttpGet("locked")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<ApplicationUserForDisplayDto>> GetLockedoutUsers()
        {
                var users = _accountsRepository.GetLockedOutUsers();
                return Ok(users);
        }
        /// <summary>
        /// Endpoint responsible for locking out users
        /// </summary>
        /// <returns>Ok if successfule.</returns>
        [SwaggerOperation(Summary = "Lock-out all users")]
        [HttpPost("lockoutUsers")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<ApplicationUserForDisplayDto>>> LockoutUsers([FromForm] int minutes)
        {
                var _ = await _accountsRepository.LockoutUsers(minutes);
                return Ok();
        }
        /// <summary>
        /// Endpoint responsible for unlocking all users
        /// </summary>
        /// <returns>Ok if successfule.</returns>
        [SwaggerOperation(Summary = "Lock-out all users")]
        [HttpPost("unlockoutUsers")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<ApplicationUserForDisplayDto>>> UnLockUsers([FromForm] int minutes)
        {
                var _ = await _accountsRepository.UnLockoutUsers();
                return Ok();
        }
        /// <summary>review
        /// Endpoint responsible for locking out users
        /// </summary>
        /// <returns> 
        [SwaggerOperation(Summary = "Lockout a single user")]
        [HttpPost("lockoutUser")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> LockOutUser([FromForm] string username, [FromForm] int minutes)
        {
                if (await _accountsRepository.LockoutUser(username, minutes))
                        return Ok();
                return BadRequest("Something went wrong");
        }
        /// <summary>
        /// Endpoint responsible for locking out users
        /// </summary>
        /// <returns> 
        [SwaggerOperation(Summary = "Flag a user")]
        [HttpPost("flagUser")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> FlagUser([FromForm]string username)
        {
                if (await _accountsRepository.FlagUser(username))
                        return Ok();
                return BadRequest("Something went wrong");
        }
        /// <summary>review
        /// Endpoint responsible for unlocking users
        /// </summary>
        /// <returns> 
        [SwaggerOperation(Summary = "Unlock user account")]
        [HttpPost("unlockoutUser")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UnlockUser([FromForm] string username, [FromForm] int minutes)
        {
                if (await _accountsRepository.UnlockUser(username))
                        return Ok();
                return BadRequest("Something went wrong");
        }
        /// <summary>
        /// Endpoint responsible for unflagging users
        /// </summary>
        /// <returns> 
        [SwaggerOperation(Summary = "Unflag a user\'s account")]
        [HttpPost("unflagUser")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<ApplicationUserForDisplayDto>>> UnFlagUser([FromForm] string username)
        {
                if (await _accountsRepository.UnflagUser(username))
                        return Ok();
                return BadRequest("Something went wrong");
        }
}