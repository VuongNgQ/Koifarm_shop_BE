using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateFishSubImageDTO
    {
        public IEnumerable<IFormFile> SubImageFile { get; set; }
        public int FishId { get; set; }
    }
}
