using _RegPage.Model;
using Microsoft.EntityFrameworkCore;

namespace _RegPage.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Credentials> Credentials { get; set; }
    }
}
