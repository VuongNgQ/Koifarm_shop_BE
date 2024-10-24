using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseSubImageDTO
    {
        public int SubImageId { get; set; }
        public string SubImageURL { get; set; }
        public int? FishId { get; set; }
        public int? FishPackageId { get; set; }
    }
}
