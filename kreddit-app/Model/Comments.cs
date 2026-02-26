using System.ComponentModel.DataAnnotations;

namespace kreddit_app.Model;

public class Comments
{
    public Comments() { }
    public Comments(string username, int upVotes, int downVotes, string content, DateTime date)
    {
        this.Username = username;
        this.UpVotes = upVotes; 
        this.DownVotes = downVotes;
        this.Content = content;
        this.Date = date;
    }
    
    [Key]
    public int CommentId { get; set; }
    public int PostId { get; set; }

    public Posts Post { get; set; }

    public DateTime  Date { get; set; }
    public int UpVotes { get; set; }
    public int DownVotes { get; set; }
    public string? Content { get; set; }
    public string Username { get; set; }
}