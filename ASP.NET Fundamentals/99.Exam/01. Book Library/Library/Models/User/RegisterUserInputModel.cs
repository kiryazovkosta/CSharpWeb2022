namespace Library.Models.User
{
    using System.ComponentModel.DataAnnotations;

    using static Library.Common.GlobalData.User;

    public class RegisterUserInputModel
    {
        [Required]
        [StringLength(MaxUserNameLength, MinimumLength = MinUserNameLength)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(MaxEmailLength, MinimumLength = MinEmailLength)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(MaxUserNameLength, MinimumLength = MinUserNameLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
