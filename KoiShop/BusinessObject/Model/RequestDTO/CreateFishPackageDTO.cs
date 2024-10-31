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
        [Required(ErrorMessage ="Name must not be null")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age must not be null")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Gender must not be null")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Size must not be null")]
        public decimal Size { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Total Price must not be null")]
        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "Daily Food must not be null")]
        public decimal DailyFood { get; set; }
        // Use IFormFile for local file uploads
        [Required(ErrorMessage = "Image must not be null")]
        public IFormFile? ImageUrl { get; set; }
        [Required(ErrorMessage = "Number must not be null")]
        public int NumberOfFish { get; set; }
        
    }
}
