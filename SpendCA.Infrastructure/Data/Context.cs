using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpendCA.Infrastructure.Data.Entities;
using SpendCA.Core.Entities;

namespace SpendCA.Infrastructure.Data
{
    public class Context : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Spend> Spends { get; set; }
    }
}
