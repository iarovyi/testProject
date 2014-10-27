using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.Domain
{
    public partial class Blog
    {
        public Blog()
        {
            this.Posts = new List<Post>();
        }


        public int BlogId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
