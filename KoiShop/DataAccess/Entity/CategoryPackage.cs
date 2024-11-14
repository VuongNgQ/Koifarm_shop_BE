using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class CategoryPackage
    {
        public int FishPackageId { get; set; }
        public int CategoryId { get; set; }
        public int QuantityOfEach { get; set; }

        public Category Category { get; set; }
        public FishPackage Package { get; set; }
    }
}
