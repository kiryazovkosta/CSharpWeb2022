using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Library.Common.GlobalData.Book;
using Library.Models.Category;

namespace Library.Models.Book
{
    public class BookInputModel
    {
        [Required]
        [StringLength(MaxTitleLength, MinimumLength = MinTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(MaxAuthorLength, MinimumLength = MinAuthorLength)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(MaxDescriptionLength, MinimumLength = MinDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), MinRatingLength, MaxRatingLength, ConvertValueInInvariantCulture = true)]
        public decimal Rating { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewNodel> Categories { get; set; }
            = new HashSet<CategoryViewNodel>();
    }
}
