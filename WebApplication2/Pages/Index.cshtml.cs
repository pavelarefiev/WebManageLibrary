using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Pages.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // Добавляем IWebHostEnvironment
using System;
using Microsoft.Extensions.Logging;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(AppDbContext context, IWebHostEnvironment environment, ILogger<IndexModel> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        [BindProperty]
        public Book Book { get; set; }

        [BindProperty]
        public IFormFile File { get; set; }

        public IList<Book> Books { get; set; }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("OnGetAsync started");
            Books = await _context.Books.ToListAsync();
            _logger.LogInformation("OnGetAsync finished");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync started");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                return Page();
            }
            _logger.LogInformation("Model state is valid.");

            if (File != null && File.Length > 0)
            {
                // Проверка типа файла
                if (!File.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("File", "Файл должен быть PDF.");
                    _logger.LogWarning("File is not pdf.");
                    return Page();
                }
                // Проверка размера файла (не более 5Мб)
                if (File.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "Файл не должен превышать 5 Мб.");
                    _logger.LogWarning("File is too large.");
                    return Page();
                }

                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    _logger.LogInformation($"Created directory {uploadsFolder}");
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    _logger.LogInformation($"Start copy file to {filePath}");
                    await File.CopyToAsync(stream);
                    _logger.LogInformation($"Copy file to {filePath} successfull");
                }
                Book.FilePath = "/uploads/" + fileName;
                _logger.LogInformation($"File path is {Book.FilePath}");
            }
            else
            {
                ModelState.AddModelError("File", "Файл не выбран");
                _logger.LogWarning("File is not selected.");
                return Page();
            }


            _context.Books.Add(Book);
            try
            {
                _logger.LogInformation("Start saving changes");
                await _context.SaveChangesAsync();
                _logger.LogInformation("Saving changes successfull");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ошибка при сохранении данных: {ex.Message}");
                _logger.LogError($"Error during save changes: {ex.Message}");
                return Page();
            }

            _logger.LogInformation("OnPostAsync finished");
            return RedirectToPage();
        }
    }
}