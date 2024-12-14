using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.Extensions.Logging;

namespace WebApplication2.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(AppDbContext context, IWebHostEnvironment environment, ILogger<CreateModel> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
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
            _logger.LogInformation("OnPostAsync method started");
            _logger.LogInformation("ModelState valid: " + ModelState.IsValid.ToString());

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");
                return Page();
            }

            try
            {
                if (UploadedFile != null && UploadedFile.Length > 0)
                {
                    _logger.LogInformation("Starting file processing");
                    // Проверка типа файла
                    _logger.LogInformation($"Content type {UploadedFile.ContentType}");
                    if (!UploadedFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase) && !UploadedFile.ContentType.Equals("text/plain", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("File", "Файл должен быть PDF или TXT.");
                        _logger.LogWarning("File is not a PDF or TXT.");
                        return Page();
                    }
                    // Проверка размера файла (не более 5Мб)
                    if (UploadedFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("File", "Файл не должен превышать 5 Мб.");
                        _logger.LogWarning("File is too large.");
                        return Page();
                    }

                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    _logger.LogInformation($"Upload folder path: {uploadsFolder}");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        _logger.LogInformation($"Create upload folder {uploadsFolder}");
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadedFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    _logger.LogInformation($"File path: {filePath}");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        _logger.LogInformation("Start copy file");
                        try
                        {
                            await UploadedFile.CopyToAsync(stream);
                            _logger.LogInformation("Copy file successfull");
                        }
                        catch (Exception copyEx)
                        {
                            _logger.LogError($"An error occurred during file copy: {copyEx.Message}\n{copyEx.StackTrace}");
                            throw;
                        }
                    }
                    Book.FilePath = "/uploads/" + fileName;
                }
                else if (string.IsNullOrEmpty(Book.FilePath))
                {
                    ModelState.AddModelError("Book.FilePath", "Введите путь к файлу или загрузите файл.");
                    _logger.LogWarning("File path is empty");
                    return Page();
                }

                _logger.LogInformation("Saving to database");
                _context.Books.Add(Book);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Saving to database successfull");
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during file upload or database operation: {ex.Message}\n{ex.StackTrace}");
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}