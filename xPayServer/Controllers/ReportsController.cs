using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using xPayServer.Models;
using Microsoft.AspNetCore.Authorization;

namespace xPayServer.Controllers;

/// <summary>
/// Provides functionality for account creation and login for admin to the /Admin/Auth route.
/// </summary>
/// 
[Authorize(Roles = "admin")]
[Route("api/Admin/Reports")]
[ApiController]
public class ReportController : ControllerBase
{
        private readonly UserManager<ApplicationUser> _userManager;
        public ReportController(UserManager<ApplicationUser> userManager)
        {
                _userManager = userManager;

        }
        [HttpGet]
        public IActionResult Get()
        {
                var stream =  System.IO.File.OpenRead(@"./Documents/report.pdf");
                return File(stream, "application/octet-stream", "OctoberFinancialReport.pdf");
        }
}