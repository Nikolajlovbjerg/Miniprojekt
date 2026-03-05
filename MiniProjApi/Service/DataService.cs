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
        if (db.Posts.Any())
        {
            return; 
        }

        // 1. Create a Post
        var post = new Posts("Hej hej det virker", DateTime.Now.AddDays(-2), "Guru", 10, 2)
        {
            Content = "I am god"
        };

        // 2. Add some comments to it
        post.Comments.Add(new Comments("Jack", 5, 0, "Cool", DateTime.Now.AddDays(-1)));
        post.Comments.Add(new Comments("Jens", 2, 1000, "Trash", DateTime.Now.AddHours(-5)));

        // 4. Add to DB and Save
        db.Posts.AddRange(post);
        db.SaveChanges();
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

    public void VoteComment(int postid, int commentId, bool upvote)
    {
        var post = db.Posts
            .Include(p => p.Comments)
            .FirstOrDefault(p => p.PostId == postid);

        if (post != null)
        {
            // 2. Find the specific comment inside that post's collection
            var comment = post.Comments.FirstOrDefault(c => c.CommentId == commentId);

            if (comment != null)
            {
                // 3. Apply the vote to that specific comment object
                if (upvote) comment.UpVotes++; else comment.DownVotes++;
                db.SaveChanges();
            }
        }
    }
}