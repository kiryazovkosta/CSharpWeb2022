using System.ComponentModel.DataAnnotations;

namespace Library.Models.ApplicationUserBook
{
    public class UserBookInputModel
    {
        [Required]
        public string UserId { get; set; } = null!;

        public int BookId { get; set; }
    }
}
