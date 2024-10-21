using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string email, string resetLink);
        //public async Task SendResetPasswordEmail(string email, string token)
        //{
        //    var resetLink = $"https://yourfrontend.com/reset-password?token={token}";
        //    var message = $"Please click the link to reset your password: {resetLink}";

        //    // Gửi email logic
        //}
    }

}
