using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class UpdateUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public String Password { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    public class UpdateProfileDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool ChangePassword { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

}
