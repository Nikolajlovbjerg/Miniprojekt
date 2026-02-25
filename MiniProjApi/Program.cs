using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniProjApi.Data;
using MiniProjApi.Service;
using MiniProjApi.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PostsContext>(options =>
    options.UseSqlite("Data Source=bin/MiniProjApi.db"));

// Registrer DataService så den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PostsContext>();
    db.Database.EnsureCreated();
    if (!db.Posts.Any())
    {
        var service = scope.ServiceProvider.GetRequiredService<DataService>();
        service.CreatePost("Velkommen til Reddit!", "Dette er den første tråd.", null, "Admin");
    }
}


app.MapGet("/api/posts", (DataService service) => service.GetPosts());

app.MapGet("/api/posts/{id}", (DataService service, int id) => {
    var post = service.GetPost(id);
    return post is not null ? Results.Ok(post) : Results.NotFound();
});

app.MapPost("/api/posts", (DataService service, CreatePostRequest req) => {
    service.CreatePost(req.Title, req.Content, req.Link, req.Username);
    return Results.Created();
});


app.MapPost("/api/posts/{id}/comments", (DataService service, int id, CreateCommentRequest req) => {
    service.AddComment(id, req.Text, req.Username);
    return Results.Created();
});


app.MapPut("/api/posts/{id}/vote", (DataService service, int id, bool upvote) => {
    service.VotePost(id, upvote);
    return Results.NoContent();
});

app.Run();

public record CreatePostRequest(string Title, string? Content, string? Link, string Username);
public record CreateCommentRequest(string Text, string Username);