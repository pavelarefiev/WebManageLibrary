// Pages/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using WebApplication1.Pages.Models;
namespace WebApplication1.Pages.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }

    }
}
