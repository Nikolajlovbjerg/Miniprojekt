using System.ComponentModel.DataAnnotations;
using kreddit;

namespace kreddit_app.Model;

public class Posts
{
    public Posts(string Title, DateTime Date, string username, int upVotes, int downVotes)
    {
        this.Title = Title;
        this.Date = Date;
        this.Username = username;
        this.UpVotes = upVotes;
        this.DownVotes = downVotes;
        Comments = new List<Comments>();
    }
        
    [Key]
    public int PostId { get; set; }
    public string Username { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; } 
    public string? Link { get; set; }    
    public DateTime Date { get; set; }
    public int UpVotes { get; set; }
    public int DownVotes { get; set; }
        
    public List<Comments> Comments { get; set; } = new List<Comments>();
}