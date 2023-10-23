using AutoMapper;
using Microsoft.AspNetCore.Identity;
using xPayServer.Models;
using xPayServer.Models.Dtos;

namespace xPayServer.Services;

public class AccountsRepository : IAccountsRepository
{
        public UserManager<ApplicationUser> UserMgr { get; set; }
        public IMapper Mapper { get; set; }


        public AccountsRepository(UserManager<ApplicationUser> userMgr, IMapper mapper)
        {
                UserMgr = userMgr;
                Mapper = mapper;
        }
        public async Task<bool> FlagUser(string userName)
        {
                var user = UserMgr.Users.Where((user) => user.UserName == userName).First();
                if (user is not null)
                {
                        user.Flag = true;
                        await Save(user);
                        return true;
                }
                else
                {
                        return false;
                }
        }

        public IEnumerable<ApplicationUserForDisplayDto> GetFlaggedUsers()
        {
                var users = UserMgr.Users.Where((user) => user.Flag == true).ToList();
                var usersToReturn = Mapper.Map<IEnumerable<ApplicationUserForDisplayDto>>(users);
                return usersToReturn;
        }

        public IEnumerable<ApplicationUserForDisplayDto> GetAllUsers()
        {
                var users = UserMgr.Users.ToList();
                var usersToReturn = Mapper.Map<IEnumerable<ApplicationUserForDisplayDto>>(users);
                return usersToReturn;
        }

        public IEnumerable<ApplicationUserForDisplayDto> GetLockedOutUsers()
        {
                var users = UserMgr.Users.Where((user) => user.LockoutEnabled == true).ToList();
                var usersToReturn = Mapper.Map<IEnumerable<ApplicationUserForDisplayDto>>(users);
                return usersToReturn;
        }

        public ApplicationUser GetUserByUsername(string username)
        {
                var user = UserMgr.Users.Where((user) => user.UserName == username).FirstOrDefault();
                if (user == null)
                {
                        return null;
                }

                return user;
        }

        public async Task<bool> LockoutUser(string userName, int minutes)
        {
                var user = GetUserByUsername(userName);
                if (user is not null)
                {
                        user.LockoutEnabled = true;
                        user.LockoutEnd = DateTimeOffset.Now.AddMinutes(minutes);
                        await Save(user);
                        return true;
                }
                return false;
        }
        public async Task<bool> LockoutUsers(int minutes)
        {
                foreach (var user in UserMgr.Users)
                {
                        user.LockoutEnabled = true;
                        user.LockoutEnd = DateTimeOffset.Now.AddMinutes(minutes);
                        await Save(user);
                }
                return true;
        }
        public async Task<bool> UnLockoutUsers()
        {
                foreach (var user in UserMgr.Users)
                {
                        user.LockoutEnabled = false;
                        user.LockoutEnd = null;
                        await Save(user);
                }
                return true;
        }

        public async Task<bool> Save(ApplicationUser user)
        {
                await UserMgr.UpdateAsync(user);
                return true;
        }

        public async Task<bool> UnflagUser(string userName)
        {
                var user = GetUserByUsername(userName);
                if (user is not null)
                {
                        user.Flag = false;
                        await Save(user);
                        return true;
                }
                return false;
        }

        public async Task<bool> UnlockUser(string userName)
        {
                var user = GetUserByUsername(userName);
                if (user is not null)
                {
                        user.LockoutEnabled = false;
                        await Save(user);
                        return true;
                }
                return false;
        }
}