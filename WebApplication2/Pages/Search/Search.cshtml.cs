using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Pages.Models; // Замените на ваш namespace

namespace WebApplication2.Pages
{
    public class SearchModel : PageModel
    {
        private readonly LibraryDbContext _context;

        public SearchModel(LibraryDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SearchTerm { get; set; }
        public List<FileRecord> SearchResults { get; set; }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                SearchResults = await _context.FileRecords
                    .Where(x => x.Author.Contains(SearchTerm) || x.Title.Contains(SearchTerm))
                    .ToListAsync();
            }
            return Page();
        }
    }
}