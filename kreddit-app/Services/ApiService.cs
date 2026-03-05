using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using kreddit_app.Model;
using kreddit_app.Pages;

namespace kreddit_app.Data;

public class ApiService
{
    private readonly HttpClient http;
    private readonly IConfiguration configuration;
    private readonly string baseAPI = "";

    public ApiService(HttpClient http, IConfiguration configuration)
    {
        this.http = http;
        this.configuration = configuration;
        this.baseAPI = configuration["base_api"];
    }

    public async Task<Posts[]> GetPosts()
    {
        string url = $"{baseAPI}posts/";
        return await http.GetFromJsonAsync<Posts[]>(url);
    }

    public async Task<Posts> GetPost(int id)
    {
        string url = $"{baseAPI}posts/{id}/";
        return await http.GetFromJsonAsync<Posts>(url);
    }

    public async Task<Comments> CreateComment(string content, int postId, string username)
    {
        string url = $"{baseAPI}posts/{postId}/comments";
     
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { content, username });

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Comment object
        Comments? newComment = JsonSerializer.Deserialize<Comments>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new comment 
        return newComment;
    }

    public async Task<Posts> UpvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/upvote/";

        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        string json = await msg.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<Posts>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
    public async Task<Posts?> DownvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/downvote/";

        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        string json = await msg.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<Posts>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<Comments> VoteComment(int postId, int commentId)
    {
        string url = $"{baseAPI}posts/{postId}/comments/{commentId}/upvote/";
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, "");
        string json = msg.Content.ReadAsStringAsync().Result;
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }
        Comments? updatedComment = JsonSerializer.Deserialize<Comments>(json, new JsonSerializerOptions 
        {
            PropertyNameCaseInsensitive = true
        });
        return updatedComment;
    }

    public async Task<Posts> CreatePost(string titel, string content, string username)
    {
        string url = $"{baseAPI}posts";
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { titel, content, username });

        if (msg.IsSuccessStatusCode)
        {
            // USE await HERE, NOT .Result
            string json = await msg.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<Posts>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        return null;
    }
}
