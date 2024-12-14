using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication2.Pages.Models; // Замените на ваш namespace

namespace WebApplication2.Pages // Замените на ваш namespace
{
    public class DeleteModel : PageModel
    {
        private readonly LibraryDbContext _context;

        public DeleteModel(LibraryDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FileRecord FileRecord { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileRecord = await _context.FileRecords.FirstOrDefaultAsync(m => m.Id == id);

            if (fileRecord == null)
            {
                return NotFound();
            }
            FileRecord = fileRecord;
            return Page();

        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileRecord = await _context.FileRecords.FindAsync(id);
            if (fileRecord != null)
            {
                _context.FileRecords.Remove(fileRecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}