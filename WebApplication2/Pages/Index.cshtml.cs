using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Pages.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(AppDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Book> Books { get; set; } = new List<Book>();
        [BindProperty(SupportsGet = true)]
        public string? SearchTitle { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchAuthor { get; set; }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("OnGetAsync started");
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTitle))
            {
                query = query.Where(b => b.Title.Contains(SearchTitle));
            }

            if (!string.IsNullOrEmpty(SearchAuthor))
            {
                query = query.Where(b => b.Author.Contains(SearchAuthor));
            }

            Books = await query.ToListAsync();
            _logger.LogInformation("OnGetAsync finished");
        }
    }
}