using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication2.Pages.Test
{
    public class TestModel : PageModel
    {
        private readonly ILogger<TestModel> _logger;

        public TestModel(ILogger<TestModel> logger)
        {
            _logger = logger;
            _logger.LogInformation("TestModel constructor called");
        }


        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        public void OnGet()
        {
            _logger.LogInformation("Test OnGet called");
        }

        public IActionResult OnPost()
        {
            _logger.LogInformation("Test OnPost called");
            return RedirectToPage("/Index");
        }
    }
}