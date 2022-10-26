namespace Library.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Library.Common.GlobalData.Book;

    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(MaxAuthorLength)]
        public string Author { get; set; } = null!;

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Precision(5, 2)]
        public decimal Rating { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<ApplicationUserBook> ApplicationUsersBooks { get; set; }
            = new HashSet<ApplicationUserBook>();
    }
}

