using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WebApplication1.Pages.Data;
using WebApplication1.Pages.Models;
using System.IO;
using System.Threading.Tasks;
using WebApplication1.Pages.Data;
using WebApplication1.Pages.Models;

namespace WebApplication1.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; }

        [BindProperty]
        public IFormFile PdfFile { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (PdfFile != null)
            {
                var filePath = Path.Combine("uploads", PdfFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PdfFile.CopyToAsync(stream);
                }
                Book.PdfFilePath = filePath;
            }

            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
