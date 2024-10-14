using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int? UserId { get; set; }
        public int? FishId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? AddedDate { get; set; }

        public User? User { get; set; }
        public Fish? Fish { get; set; }
        public FishPackage? Package { get; set; }
    }
}
