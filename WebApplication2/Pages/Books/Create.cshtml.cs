using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace WebApplication2.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Book Book { get; set; }

        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                // Проверка типа файла
                if (!UploadedFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("File", "Файл должен быть PDF.");
                    return Page();
                }
                // Проверка размера файла (не более 5Мб)
                if (UploadedFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "Файл не должен превышать 5 Мб.");
                    return Page();
                }
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadedFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }
                Book.FilePath = "/uploads/" + fileName;

            }
            else if (string.IsNullOrEmpty(Book.FilePath))
            {
                ModelState.AddModelError("Book.FilePath", "Введите путь к файлу или загрузите файл.");
                return Page();
            }

            _context.Books.Add(Book);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Index"); //  Изменено здесь
        }
    }
}