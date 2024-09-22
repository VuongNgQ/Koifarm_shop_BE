using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class ProductStatus
    {
        public int ProductStatusId { get; set; }
        public string? Name { get; set; }
        public ICollection<FishSingle>? Fish { get; set; }
    }
}
