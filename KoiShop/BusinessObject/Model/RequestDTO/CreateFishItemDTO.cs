using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateFishItemDTO
    {
        public int CartId { get; set; }
        public int FishId { get; set; }
        public int Quantity { get; set; }

    }
}
