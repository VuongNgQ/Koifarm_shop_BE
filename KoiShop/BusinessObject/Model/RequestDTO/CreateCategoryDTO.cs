using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage ="Name must not be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description must not be empty")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Origin must not be empty")]
        public string OriginCountry { get; set; }
    }
}
