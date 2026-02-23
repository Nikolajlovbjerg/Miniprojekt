using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Tilføj CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        policy =>
        {
            policy.WithOrigins("https://example.com") // Din frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Brug CORS
app.UseCors("AllowMyOrigin");

app.MapControllers();

app.Run();