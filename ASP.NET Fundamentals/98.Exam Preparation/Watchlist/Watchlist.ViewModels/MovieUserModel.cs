using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchlist.ViewModels
{
    public class MovieUserModel
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public int MovieId { get; set; }
    }
}
