using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreatePackageItemDTO
    {
        public int CartId { get; set; }
        public int PackageId { get; set; }
    }
}
