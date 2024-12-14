using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Pages.Models
{
    public class FileRecord
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя файла обязательно")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Путь к файлу обязателен")]
        public string FilePath { get; set; }

        [Required(ErrorMessage = "Автор обязателен")]
        [StringLength(100, ErrorMessage = "Длина автора не должна превышать 100 символов.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(250, ErrorMessage = "Длина названия не должна превышать 250 символов.")]
        public string Title { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}