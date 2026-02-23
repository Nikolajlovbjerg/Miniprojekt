using Microsoft.EntityFrameworkCore;
using MiniProjApi.Data;
using MiniProjApi.Model;

namespace MiniProjApi.Service;

public class DataService
{
    private readonly PostsContext db;

    public DataService(PostsContext db)
    {
        this.db = db;
    }

    // Henter alle tråde sorteret efter nyeste dato
    public List<Posts> GetPosts()
    {
        return db.Posts
            .OrderByDescending(p => p.Date)
            .ToList();
    }

    // Henter en specifik tråd inklusiv alle dens kommentarer
    public Posts? GetPost(int id)
    {
        return db.Posts
            .Include(p => p.Comments)
            .FirstOrDefault(p => p.PostId == id);
    }

    // Opretter en ny tråd
    public void CreatePost(string title, string? content, string? link, string username)
    {
        var post = new Posts(title, DateTime.Now, username, 0, 0)
        {
            Content = content,
            Link = link
        };
        db.Posts.Add(post);
        db.SaveChanges();
    }

    // Tilføjer en kommentar til en tråd
    public void AddComment(int postId, string text, string username)
    {
        var post = db.Posts.Find(postId);
        if (post != null)
        {
            post.Comments.Add(new Comments(username, 0, 0, text, DateTime.Now));
            db.SaveChanges();
        }
    }

    // Stemme-logik (Upvote/Downvote)
    public void VotePost(int postId, bool isUpvote)
    {
        var post = db.Posts.Find(postId);
        if (post != null)
        {
            if (isUpvote) post.UpVotes++; else post.DownVotes++;
            db.SaveChanges();
        }
    }
}