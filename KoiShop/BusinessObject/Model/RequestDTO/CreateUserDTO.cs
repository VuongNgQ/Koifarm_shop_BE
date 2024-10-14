using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class CreateUserDTO
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
        public required string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
