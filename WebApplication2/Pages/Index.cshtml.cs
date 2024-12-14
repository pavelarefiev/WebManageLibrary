using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Pages.Models;
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
                _logger.LogInformation($"SearchTitle is not empty {SearchTitle}");
            }

            if (!string.IsNullOrEmpty(SearchAuthor))
            {
                query = query.Where(b => b.Author.Contains(SearchAuthor));
                _logger.LogInformation($"SearchAuthor is not empty {SearchAuthor}");
            }

            FileRecords = await query.ToListAsync();
            _logger.LogInformation($"Found {FileRecords.Count} FileRecords");
            _logger.LogInformation("OnGetAsync finished");
        }
    }
}