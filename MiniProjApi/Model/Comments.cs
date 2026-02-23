namespace MiniProjApi.Model;

public class Comments
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public DateTime  Date { get; set; }
    public int UpVotes { get; set; }
    public int DownVotes { get; set; }
    public string Content { get; set; }
    public string UserName { get; set; }
}