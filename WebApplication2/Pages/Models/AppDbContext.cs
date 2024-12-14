using Microsoft.EntityFrameworkCore;
using WebApplication2.Pages.Models;

namespace WebApplication2.Pages
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
