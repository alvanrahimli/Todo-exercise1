using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ToDo_exercise1.Utilities
{
    public static class Helper
    {
        public static string GetEmail(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return claims[1].Value;
        }

        public static byte[] ComputeHash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] passHash = sha256Hash
                    .ComputeHash(Encoding.UTF8.GetBytes(password));
                return passHash;
            }
        }
    }
}