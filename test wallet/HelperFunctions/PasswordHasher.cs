using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using test_wallet.HelperFunctions.Implementation;

namespace test_wallet.HelperFunctions
{
    public class CustomPasswordHasher : ICustomPasswordHasher
    {
        
        public string hashPassword(string password)
        {
            using(var sha = SHA256.Create())
            {
                var passwordAsByte = Encoding.Default.GetBytes(password);
                var hashedPassword = Convert.ToBase64String(sha.ComputeHash(passwordAsByte));
                return hashedPassword;
            }
        }
    }
}
