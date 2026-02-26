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

    public void SeedData()
    {

    }


    public List<Posts> GetPosts()
    {
        return db.Posts
            .OrderByDescending(p => p.Date)
            .ToList();
    }

    public Posts? GetPost(int id)
    {
        return db.Posts
            .Include(p => p.Comments)
            .FirstOrDefault(p => p.PostId == id);
    }

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

    public void AddComment(int postId, string text, string username)
    {
        var post = db.Posts.Find(postId);
        if (post != null)
        {
            post.Comments.Add(new Comments(username, 0, 0, text, DateTime.Now));
            db.SaveChanges();
        }
    }

    public void VotePost(int postId, bool isUpvote)
    {
        var post = db.Posts.Find(postId);
        if (post != null)
        {
            if (isUpvote) post.UpVotes++; else post.DownVotes++;
            db.SaveChanges();
        }
    }

    public void VoteComment(int commentId, bool upvote)
    {
        var comment = db.Set<Comments>().Find(commentId);

        if (comment != null)
        {
            if (upvote) comment.UpVotes++; else comment.DownVotes++;
            db.SaveChanges();
        }
    }
}