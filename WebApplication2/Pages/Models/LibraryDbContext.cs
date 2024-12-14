using Microsoft.EntityFrameworkCore;
using WebApplication2.Pages.Models; // Замените на ваш namespace

namespace WebApplication2 // Замените на ваш namespace
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
        public DbSet<FileRecord> FileRecords { get; set; }
    }
}