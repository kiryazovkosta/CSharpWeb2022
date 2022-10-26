namespace Library.Models.Book
{
    using System.ComponentModel.DataAnnotations;

    public class BookViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;

        public decimal Rating { get; set; }

        [Required]
        public string Category { get; set; } = null!;
    }
}
