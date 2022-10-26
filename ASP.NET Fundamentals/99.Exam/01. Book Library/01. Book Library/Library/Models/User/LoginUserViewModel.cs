namespace Library.Models.User
{
    using System.ComponentModel.DataAnnotations;

    public class LoginUserViewModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
