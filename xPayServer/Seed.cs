using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using xPayServer.Data;
using xPayServer.Models;

namespace xPayServer;
public class Seed
{
        public static async Task SeedAdmin(IApplicationBuilder app)
        {
                using var scope = app?.ApplicationServices.CreateScope();
                if (scope is not null)
                {
                        UserManager<ApplicationUser> userManager =
                        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                        RoleManager<IdentityRole> roleManager =
                        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                        var user = new ApplicationUser
                        {
                                Id = Guid.NewGuid().ToString(),
                                UserName = "admin",
                                NormalizedUserName = "admin@test.com",
                                Email = "admin@test.com",
                                NormalizedEmail = "admin@test.com",
                                EmailConfirmed = true,
                                LockoutEnabled = false,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                LastLogin = DateTime.Now,
                                Flag = false,
                                Savings = 0
                        };

                        var roleStore = new RoleStore<IdentityRole>(context);
                        if (!context.Roles.Any(r => r.Name == "admin"))
                        {
                                await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "admin" });
                        }

                        if (!context.Users.Any(u => u.UserName == user.UserName))
                        {
                                var password = new PasswordHasher<ApplicationUser>();
                                var hashed = password.HashPassword(user, "1234");
                                user.PasswordHash = hashed;
                                var userStore = new UserStore<ApplicationUser>(context);
                                await userManager.CreateAsync(user);
                                await userManager.AddToRoleAsync(user, "admin");
                        }

                        await context.SaveChangesAsync();
                }
        }
        public static async Task SeedCustomer(IApplicationBuilder app)
        {
                using var scope = app?.ApplicationServices.CreateScope();
                if (scope is not null)
                {
                        UserManager<ApplicationUser> userManager =
                        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                        RoleManager<IdentityRole> roleManager =
                        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                        List<ApplicationUser> userList = new() {
                        new ApplicationUser
                        {
                                Id = Guid.NewGuid().ToString(),
                                UserName = "cust1",
                                NormalizedUserName = "cust1@test.com",
                                Email = "cust1@test.com",
                                NormalizedEmail = "cust1@test.com",
                                EmailConfirmed = true,
                                LockoutEnabled = false,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                LastLogin = DateTime.Now,
                                Flag = false,
                                Savings = 200000
                        },
                        new ApplicationUser
                        {
                                Id = Guid.NewGuid().ToString(),
                                UserName = "cust2",
                                NormalizedUserName = "cust2@test.com",
                                Email = "cust2@test.com",
                                NormalizedEmail = "cust2@test.com",
                                EmailConfirmed = true,
                                LockoutEnabled = false,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                LastLogin = DateTime.Now,
                                Flag = false, Savings = 500000
                        },
                        new ApplicationUser
                        {
                                Id = Guid.NewGuid().ToString(),
                                UserName = "cust3",
                                NormalizedUserName = "cust3@test.com",
                                Email = "cust3@test.com",
                                NormalizedEmail = "cust3@test.com",
                                EmailConfirmed = true,
                                LockoutEnabled = false,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                LastLogin = DateTime.Now,
                                Flag = false, Savings = 20000000
                        },
                        new ApplicationUser
                        {
                                Id = Guid.NewGuid().ToString(),
                                UserName = "cust4",
                                NormalizedUserName = "cust4@test.com",
                                Email = "cust4@test.com",
                                NormalizedEmail = "cust4@test.com",
                                EmailConfirmed = true,
                                LockoutEnabled = false,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                LastLogin = DateTime.Now,
                                Flag = false, Savings = 5000
                        },
                        new ApplicationUser
                        {
                                Id = Guid.NewGuid().ToString(),
                                UserName = "cust5",
                                NormalizedUserName = "cust5@test.com",
                                Email = "cust5@test.com",
                                NormalizedEmail = "cust5@test.com",
                                EmailConfirmed = true,
                                LockoutEnabled = false,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                LastLogin = DateTime.Now,
                                Flag = false, Savings = 20000
                        },
                        new ApplicationUser
                        {
                                Id = Guid.NewGuid().ToString(),
                                UserName = "cust6",
                                NormalizedUserName = "cust6@test.com",
                                Email = "cust6@test.com",
                                NormalizedEmail = "custg@test.com",
                                EmailConfirmed = true,
                                LockoutEnabled = false,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                LastLogin = DateTime.Now,
                                Flag = false, 
                                Savings = 200000
                        }};

                        var roleStore = new RoleStore<IdentityRole>(context);
                        if (!context.Roles.Any(r => r.Name == "cust"))
                        {
                                await roleStore.CreateAsync(new IdentityRole { Name = "cust", NormalizedName = "cust" });
                        }
                        foreach (var u in userList)
                        {
                                if (!context.Users.Any(user => user.UserName == u.UserName))
                                {
                                        var password = new PasswordHasher<ApplicationUser>();
                                        var hashed = password.HashPassword(u, "1234");
                                        u.PasswordHash = hashed;
                                        var userStore = new UserStore<ApplicationUser>(context);
                                        await userManager.CreateAsync(u);
                                        await userManager.AddToRoleAsync(u, "admin");
                                }
                        }
                        await context.SaveChangesAsync();
                }
        }
}