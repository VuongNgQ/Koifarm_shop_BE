using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public String Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
