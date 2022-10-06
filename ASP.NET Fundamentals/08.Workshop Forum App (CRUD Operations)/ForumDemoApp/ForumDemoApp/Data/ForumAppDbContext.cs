namespace ForumDemoApp.Data
{
    using ForumDemoApp.Data.Configure;
    using ForumDemoApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    
    public class ForumAppDbContext : DbContext
    {
        public ForumAppDbContext(DbContextOptions<ForumAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Post>(new PostConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
