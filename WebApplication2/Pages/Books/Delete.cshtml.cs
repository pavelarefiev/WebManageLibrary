using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication2.Pages.Models;
using Microsoft.Extensions.Logging;
using System;

namespace WebApplication2.Pages.Books
{
    public class DeleteModel : PageModel
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(LibraryDbContext context, ILogger<DeleteModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        [BindProperty]
        public int? id { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _logger.LogInformation($"OnGetAsync started with id: {id}");
            if (id == null)
            {
                _logger.LogError("id is null");
                return NotFound();
            }
            var fileRecord = await _context.FileRecords.FirstOrDefaultAsync(m => m.Id == id);

            if (fileRecord == null)
            {
                _logger.LogError($"file record with id: {id} was not found");
                return NotFound();
            }
            _logger.LogInformation($"file record is found: {fileRecord.FileName}");

            ViewData["FileName"] = fileRecord.FileName;
            ViewData["Author"] = fileRecord.Author;
            ViewData["Title"] = fileRecord.Title;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation($"OnPostAsync started with id: {id}");
            if (id == null)
            {
                _logger.LogError("id is null");
                return NotFound();
            }
            try
            {
                var fileRecord = await _context.FileRecords.FindAsync(id);
                if (fileRecord != null)
                {
                    _logger.LogInformation($"file record is found: {fileRecord.FileName}");
                    _context.FileRecords.Remove(fileRecord);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"file record with id {id} was deleted");

                }
                else
                {
                    _logger.LogError($"file record with id: {id} was not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SaveChangesAsync: {ex.Message}, {ex.StackTrace}");
                // Optionally add a ModelState error
                ModelState.AddModelError(string.Empty, "Error deleting record.");
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }
}