using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Utils
{
    public class Util
    {
        public static string HashPassword(string password, string configSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(configSalt)))
            {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string configSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(configSalt)))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == storedHash;
            }
        }
        public static long GetTimeStamp(DateTime date)
        {
            return (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        public static long GetTimeStamp()
        {
            return GetTimeStamp(DateTime.Now);
        }
    }
}
