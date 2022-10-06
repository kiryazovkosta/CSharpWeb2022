using System.ComponentModel.DataAnnotations;
using static ForumDemoApp.Common.DataConstants.Post;

namespace ForumDemoApp.Models
{
    public class PostViewModel
    {
        [UIHint("hidden")]
        public int Id { get; set; }

        [Display(Name = "Заглавие")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Полето {0} трябва да е между {2} и {1} !")]
        public string Title { get; set; } = null!;

        [Display(Name = "Съдържание")]
        [Required(ErrorMessage = "Съдържание {0} е задължително")]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "Полето {0} трябва да е между {2} и {1} !")]
        public string Content { get; set; } = null!;
    }
}