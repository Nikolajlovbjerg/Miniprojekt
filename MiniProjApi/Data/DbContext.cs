using Microsoft.EntityFrameworkCore;
using MiniProjApi.Model;

namespace Data
{
    public class PostsContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Posts> Post => Set<Posts>();


        public PostsContext(DbContextOptions<PostsContext> options)
            : base(options)
        {
            // Den her er tom. Men ": base(options)" sikre at constructor
            // på DbContext super-klassen bliver kaldt.
        }
    }
}
