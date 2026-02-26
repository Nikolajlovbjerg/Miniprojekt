using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniProjApi.Data;
using MiniProjApi.Service;
using MiniProjApi.Model;

var builder = WebApplication.CreateBuilder(args);

// 1. Services
builder.Services.AddDbContext<PostsContext>(options =>
    options.UseSqlite("Data Source=bin/MiniProjApi.db"));

builder.Services.AddScoped<DataService>();
builder.Services.AddAuthorization(); 

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.PropertyNameCaseInsensitive = true; 
});

// 2. CORS - The "AllowAll" approach is safer for local dev debugging
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// 3. Pipeline Order
app.UseRouting();
app.UseCors("AllowAll"); // Must be here
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PostsContext>();
    db.Database.EnsureCreated();
}

// 5. Routes
app.MapGet("/api/posts", (DataService service) => Results.Ok(service.GetPosts()));
app.MapGet("/api/posts/{id}", (DataService service, int id) => {
    var post = service.GetPost(id);
    return post != null ? Results.Ok(post) : Results.NotFound();
});

// POST Post: Matches Blazor 'new { titel, content, username }'
app.MapPost("/api/posts", (DataService service, CreatePostRequest req) => {
    service.CreatePost(req.titel, req.content, null, req.username ?? "Anonymous");
    return Results.Created($"/api/posts", req);
});

// POST Comment: Matches Blazor 'new { content, userId }'
app.MapPost("/api/posts/{id}/comments", (DataService service, int id, CreateCommentRequest req) => {
    // We use userId as the username string since that's what Blazor is sending
    service.AddComment(id, req.content, $"User {req.userId}");
    return Results.Created($"/api/posts/{id}/comments", req);
});

app.MapPut("/api/posts/{id}/upvote", (DataService service, int id) => {
    service.VotePost(id, true);
    return Results.Ok();
});

app.Run();

// 6. DTOs - Property names MUST match the Blazor anonymous objects exactly
public record CreatePostRequest(string titel, string content, string? username);
public record CreateCommentRequest(string content, int userId);