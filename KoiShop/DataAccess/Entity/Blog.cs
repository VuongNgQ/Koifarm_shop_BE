using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}
