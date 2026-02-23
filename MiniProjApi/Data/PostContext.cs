using Microsoft.EntityFrameworkCore;
using MiniProjApi.Model;

namespace MiniProjApi.Data
{
    public class PostsContext : DbContext
    {
        /*public DbSet<User> Users => Set<User>();*/
        public DbSet<Posts> Post => Set<Posts>();

        public string DbPath { get; }

        public PostsContext()
        {
            DbPath = "bin/MiniProjApi.db";
        }

        public PostsContext(DbContextOptions<PostsContext> options)
            : base(options)
        {
            // Den her er tom. Men ": base(options)" sikre at constructor
            // på DbContext super-klassen bliver kaldt.
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Posts>().ToTable("Posts");
        }
    }
}
