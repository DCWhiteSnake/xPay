using Microsoft.AspNetCore.Identity;

namespace xPayServer.Models;
public class ApplicationUser : IdentityUser
{
        public DateTime? LastLogin{get; set;}
        public bool Flag {get; set;} = false;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public Decimal Savings {get; set;} = 1000;
        public Decimal WithdrawalLimit {get; set;} = 200000;
}