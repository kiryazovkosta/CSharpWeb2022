namespace Library.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Library.Common.GlobalData.Category;

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; } = null!;

        public ICollection<Book> books { get; set; } = new HashSet<Book>();
    }
}

