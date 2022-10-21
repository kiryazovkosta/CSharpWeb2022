using System.ComponentModel.DataAnnotations;
using static Watchlist.Common.GlobalData.Movie;

namespace Watchlist.ViewModels
{
    public class MovieInputModel
    {
        [Required]
        [StringLength(MaxTitleLength, MinimumLength = MinTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(MaxDirectorLength, MinimumLength = MinDirectorLength)]
        public string Director { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), MinRatingLength, MaxRatingLength, ConvertValueInInvariantCulture = true)]
        public decimal Rating { get; set; }

        [Required]
        public int GenreId { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; } = new HashSet<GenreViewModel>();
    }
}
