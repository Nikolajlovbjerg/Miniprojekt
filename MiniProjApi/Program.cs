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

builder.Services.AddScoped<DataService>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

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
        service.AddComment(1, "Sejt mannnnn", "Tobias");
    }
}


// GET ruter
app.MapGet("/api/posts", (DataService service) => service.GetPosts());
app.MapGet("/api/posts/{id}", (DataService service, int id) => service.GetPost(id));

// POST ruter
app.MapPost("/api/posts", (DataService service, CreatePostRequest req) => 
    service.CreatePost(req.Title, req.Content, req.Link, req.Username));

app.MapPost("/api/posts/{id}/comments", (DataService service, int id, CreateCommentRequest req) => 
    service.AddComment(id, req.Text, req.Username));

// PUT ruter (Upvote/Downvote)
app.MapPut("/api/posts/{id}/upvote", (DataService service, int id) => service.VotePost(id, true));
app.MapPut("/api/posts/{id}/downvote", (DataService service, int id) => service.VotePost(id, false));

app.MapPut("/api/posts/{postid}/comments/{commentid}/upvote", (DataService service, int postid, int commentid) => 
    service.VoteComment(commentid, true));

app.MapPut("/api/posts/{postid}/comments/{commentid}/downvote", (DataService service, int postid, int commentid) => 
    service.VoteComment(commentid, false));

app.Run();

public record CreatePostRequest(string Title, string? Content, string? Link, string Username);
public record CreateCommentRequest(string Text, string Username);