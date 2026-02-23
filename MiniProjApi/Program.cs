using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniProjApi.Data;
using MiniProjApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Add DbContext (IMPORTANT)
builder.Services.AddDbContext<PostsContext>(options =>
    options.UseSqlite("Data Source=MiniProjApi.db"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowMyOrigin");

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PostsContext>();

    // Ensure database is created
    db.Database.EnsureCreated();

    // Seed only if database is empty
    if (!db.Posts.Any())
    {
        // Create a comment
        var comment = new Comments("Peter", 77, 9, "Morten er grim", DateTime.Now);

        // Create a post and add the comment
        var post = new Posts("SupersejTittel", DateTime.Now, "Jens Jensen", 5, 100);
        post.Comments.Add(comment);

        // Add and save
        db.Posts.Add(post);
        db.SaveChanges();
    }
}


app.Run();