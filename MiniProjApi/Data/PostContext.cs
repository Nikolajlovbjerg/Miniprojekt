using Microsoft.EntityFrameworkCore;
using MiniProjApi.Model;

namespace MiniProjApi.Data;

public class PostsContext : DbContext
{
    public PostsContext(DbContextOptions<PostsContext> options)
        : base(options) { }

    public DbSet<Posts> Posts => Set<Posts>();  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comments>()
            .HasKey(c => c.CommentId);
        
        modelBuilder.Entity<Posts>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId);
    }
}