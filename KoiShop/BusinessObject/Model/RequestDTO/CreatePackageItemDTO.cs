using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreatePackageItemDTO
    {
        public int UserCartId { get; set; }
        public int PackageId { get; set; }
        public int Quantity { get; set; }
    }
}
