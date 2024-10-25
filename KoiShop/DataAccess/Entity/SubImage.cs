using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class SubImage
    {
        public int SubImageId { get; set; }
        public string SubImageURL { get; set; }
        public int? FishId { get; set; }
        public int? FishPackageId { get; set; }
        public Fish? Fish { get; set; }
        public FishPackage? FishPackage { get; set; }
    }
}
