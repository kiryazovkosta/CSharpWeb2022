using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchlist.Models
{
    public class User : IdentityUser
    {
        public ICollection<UserMovie> UserMovies { get; set; } = new HashSet<UserMovie>();
    }
}
