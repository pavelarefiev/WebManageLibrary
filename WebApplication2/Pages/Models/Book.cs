using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Pages.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [DataType(DataType.Upload)]
        public string FilePath { get; set; }
    }
}
