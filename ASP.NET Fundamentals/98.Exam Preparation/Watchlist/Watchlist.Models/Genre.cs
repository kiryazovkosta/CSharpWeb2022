namespace Watchlist.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Watchlist.Common.GlobalData.Genre;

    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; } = null!;

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}
