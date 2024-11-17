using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateFishPackageDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        /*[Required]
        public ICollection<CreateCategoryPackageDTO> Categories { get; set; }*/
        [Required]
        public decimal MinSize { get; set; }
        [Required]
        public decimal MaxSize { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }  
        [Required]
        public decimal DailyFood { get; set; }
        [Required]
        [Range(9, 101, ErrorMessage = "Capacity must be between 10 and 100")]
        public int Capacity { get; set; }
        [Required]
        public IFormFile? ImageUrl { get; set; }
        [Required]
        public int QuantityInStock { get; set; }

    }
    public class CreateCategoryPackageDTO
    {
        public int FishPackageId { get; set; }
        public int CategoryId { get; set; }
        [Range(0, 11, ErrorMessage = "Quantity must be between 1 and 10")]
        public int QuantityOfEach { get; set; }
    }
}
