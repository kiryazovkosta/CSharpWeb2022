namespace ForumDemoApp.Data.Configure
{
    using ForumDemoApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            var posts = GetPosts();
            builder.HasData(posts);
        }

        private List<Post> GetPosts()
        {
            return new List<Post>()
            {
                new Post()
                {
                    Id = 1,
                    Title = "First",
                    Content = "Content of the first post"
                },
                new Post()
                {
                    Id = 2,
                    Title = "Second",
                    Content = "Some long text content of the secons post"
                },
                new Post()
                {
                    Id = 3,
                    Title = "Third",
                    Content = "The third post The third post The third post The third post The third postThe third post The third post The third post"
                }
            };
        }
    }
}
