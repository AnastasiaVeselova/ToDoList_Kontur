using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Auth
{
    public class AuthHash
    {
        public static string GetHashPassword(string password)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5.ComputeHash(passwordBytes);
                var hash = BitConverter.ToString(hashBytes);
                return hash;
            }
        }
    }
}
