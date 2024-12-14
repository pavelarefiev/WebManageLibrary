using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Pages.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        public string? FilePath { get; set; }
    }
}