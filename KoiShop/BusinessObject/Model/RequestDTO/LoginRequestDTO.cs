using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model.RequestDTO
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RequestPasswordResetDTO
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

}
