using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjApi.Model
{
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
        
        public int PostId { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        
        public List<Comments> Comments { get; set; } = new List<Comments>();
    }
}
