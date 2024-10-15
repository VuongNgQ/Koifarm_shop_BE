using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class UpdateProfileDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public String Password { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
}

}
