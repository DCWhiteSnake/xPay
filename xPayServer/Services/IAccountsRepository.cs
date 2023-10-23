using Microsoft.AspNetCore.Identity;
using xPayServer.Models;
using xPayServer.Models.Dtos;
namespace xPayServer.Services;
public interface IAccountsRepository
{
        public  UserManager<ApplicationUser> UserMgr {get; set;}
        public ApplicationUser GetUserByUsername(string username);
        public IEnumerable<ApplicationUserForDisplayDto> GetAllUsers();
        public IEnumerable<ApplicationUserForDisplayDto> GetLockedOutUsers();
        public IEnumerable<ApplicationUserForDisplayDto> GetFlaggedUsers();
        public Task<bool> FlagUser(string userName);
        public Task<bool> LockoutUsers(int minutes);
        public Task<bool> UnLockoutUsers();
        public Task<bool> LockoutUser(string userName, int minutes);
        public Task<bool> UnlockUser(string userName);
        public Task<bool> UnflagUser(string userName);
        public Task<bool> Save(ApplicationUser user);

}