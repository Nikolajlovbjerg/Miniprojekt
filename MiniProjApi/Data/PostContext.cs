using Microsoft.EntityFrameworkCore;
using MiniProjApi.Model;

namespace MiniProjApi.Data;

public class PostsContext : DbContext
{
    public PostsContext(DbContextOptions<PostsContext> options)
        : base(options) { }

    public DbSet<Posts> Posts => Set<Posts>();  // Only DbSet

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Posts>(post =>
        {
            post.ToTable("Posts");
            post.HasKey(p => p.PostId);  // ✅ Primary key for Posts

            // Configure Comments as owned
            post.OwnsMany(p => p.Comments, c =>
            {
                c.WithOwner().HasForeignKey("PostId");  // Foreign key
                c.Property<int>("CommentId");            // Shadow PK
                c.HasKey("CommentId");                   // Set shadow PK
                c.Property(c => c.Username);
                c.Property(c => c.Content);
                c.Property(c => c.Date);
                c.Property(c => c.UpVotes);
                c.Property(c => c.DownVotes);
            });
        });
    }
}