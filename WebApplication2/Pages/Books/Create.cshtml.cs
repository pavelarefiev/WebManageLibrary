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
        private readonly LibraryDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(LibraryDbContext context, IWebHostEnvironment environment, ILogger<CreateModel> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
            _logger.LogInformation("CreateModel constructor is called");
        }

        [BindProperty]
        public FileRecord FileRecord { get; set; } = new FileRecord();

        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            _logger.LogInformation("OnPost method is called");

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is not valid");
                foreach (var modelError in ModelState.Values.SelectMany(e => e.Errors))
                {
                    _logger.LogError($"Error: {modelError.ErrorMessage}");
                }
                return Page();
            }
            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                try
                {
                    _logger.LogInformation($"File Name {UploadedFile.FileName},  File Size = {UploadedFile.Length} File Type = {UploadedFile.ContentType}");
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                        _logger.LogInformation($"Directory created at {uploadsFolder}");
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + UploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        UploadedFile.CopyTo(fileStream);
                    }
                    _logger.LogInformation($"File saved to {filePath}");
                    FileRecord.FilePath = "/uploads/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving file: {ex.Message}, {ex.StackTrace}");
                    ModelState.AddModelError(string.Empty, "Error saving file.");
                    return Page();
                }
            }
            else
            {
                FileRecord.FilePath = null;
            }

            _context.FileRecords.Add(FileRecord);
            _context.SaveChanges();

            return RedirectToPage("/Index");
        }
    }
}