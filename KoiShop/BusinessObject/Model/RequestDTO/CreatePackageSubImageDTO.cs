using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreatePackageSubImageDTO
    {
        public IEnumerable<IFormFile> SubImageFile { get; set; }
        public int FishPackageId { get; set; }
    }
}
