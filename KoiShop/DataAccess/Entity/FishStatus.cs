using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class FishStatus
    {
        public int FishStatusId {  get; set; }
        public string? StatusName { get; set; }

        public ICollection<FishConsignment>? FishConsignments { get; set; }
        public ICollection<PackageConsignment>? PackageConsignments { get; set; }
    }
}
