using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using xPayServer.Models;
namespace xPayServer.Data
{
        public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
        {
                public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
                : base(options) { }
        }
}
