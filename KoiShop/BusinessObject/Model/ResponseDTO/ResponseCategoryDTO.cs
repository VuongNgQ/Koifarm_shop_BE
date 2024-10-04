using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.ResponseDTO
{
    public class ResponseCategoryDTO
    {
        public string Name { get; set; }
        public decimal Size { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string OriginCountry { get; set; }
    }
}
