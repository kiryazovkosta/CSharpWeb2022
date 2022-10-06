namespace ForumDemoApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using static ForumDemoApp.Common.DataConstants.Post;

    [Comment("Published posts")]
    public class Post
    {
        [Comment("Unique identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Title of a post")]
        [Required]
        [StringLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Comment("Post content")]
        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;
    }
}
