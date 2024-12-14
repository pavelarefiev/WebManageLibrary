using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Pages.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string PdfFilePath { get; set; } // Путь к PDF
    }
}
