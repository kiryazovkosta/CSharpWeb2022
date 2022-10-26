using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Policy;

namespace Library.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserBook> ApplicationUsersBooks { get; set; }
            = new HashSet<ApplicationUserBook>();
    }
}

