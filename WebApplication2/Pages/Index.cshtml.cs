using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Pages.Models; // Замените на ваш namespace
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(LibraryDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<FileRecord> FileRecords { get; set; } = new List<FileRecord>();
        [BindProperty(SupportsGet = true)]
        public string? SearchTitle { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchAuthor { get; set; }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("OnGetAsync started");
            var query = _context.FileRecords.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTitle))
            {
                query = query.Where(b => b.Title.Contains(SearchTitle));
            }

            if (!string.IsNullOrEmpty(SearchAuthor))
            {
                query = query.Where(b => b.Author.Contains(SearchAuthor));
            }

            FileRecords = await query.ToListAsync();
            _logger.LogInformation("OnGetAsync finished");
        }
    }
}