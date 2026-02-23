using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjApi.Model
{
    public class Posts
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
    }
}
