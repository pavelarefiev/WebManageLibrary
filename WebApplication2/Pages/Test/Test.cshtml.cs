using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;


namespace WebApplication2.Pages.Test
{
    public class TestModel : PageModel
    {
        private readonly ILogger<TestModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public TestModel(ILogger<TestModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
            _logger.LogInformation("TestModel constructor called");
        }

        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        [BindProperty]
        public string? SelectedFile { get; set; }

        public List<string> ExistingFiles { get; set; } = new List<string>();

        public void OnGet()
        {
            _logger.LogInformation("Test OnGet called");
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            if (Directory.Exists(uploadsFolder))
            {
                ExistingFiles = Directory.GetFiles(uploadsFolder).Select(f => Path.GetFileName(f)).ToList();
            }


        }

        public IActionResult OnPost()
        {
            _logger.LogInformation("Test OnPost called");

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
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving file: {ex.Message}, {ex.StackTrace}");
                }
            }
            if (!string.IsNullOrEmpty(SelectedFile))
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                _logger.LogInformation($"Selected file {uploadsFolder}/{SelectedFile}");
            }
            else if (UploadedFile == null)
            {
                _logger.LogInformation("UploadedFile is null and SelectedFile is null");
            }
            return RedirectToPage("/Index");
        }
    }
}