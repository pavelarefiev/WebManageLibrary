using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Pages.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; }
        [BindProperty]
        public IFormFile File { get; set; }
        public IList<Book> Books { get; set; }

        public async Task OnGetAsync()
        {
            Books = await _context.Books.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (File != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder); // ������� �����, ���� ��� �� ����������
                    var filePath = Path.Combine(uploadsFolder, File.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await File.CopyToAsync(stream);
                    }

                    Book.FilePath = "/uploads/" + File.FileName; // ��������� ���� � �����
                }

                _context.Books.Add(Book);
                await _context.SaveChangesAsync();

                return RedirectToPage(); // ��������������� �� ������� ��������
            }
            catch (Exception ex)
            {
                // ����������� ������
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError(string.Empty, "��������� ������ ��� ���������� �����. ����������, ���������� ��� ���.");
                return Page();
            }
        }
    }
}