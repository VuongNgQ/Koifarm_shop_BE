using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public decimal? Size { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? OriginCountry { get; set; }
        public string? CategoryStatus { get; set; }
        public ICollection<FishSingle>? Fish { get; set; }
    }
}
