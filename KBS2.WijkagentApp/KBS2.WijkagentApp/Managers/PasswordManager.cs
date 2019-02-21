using System;
using System.Security.Cryptography;

namespace KBS2.WijkagentApp.Managers
{
    class PasswordManager
    {
        private readonly string imTheBestSaltEver = "KaboutersBestaanEcht";

        //generates a hash with a standard salt (!!!NEVER CHANGE!!!)
        public string GenerateHash(string pw)
        {
            Byte[] salt = Convert.FromBase64String(imTheBestSaltEver);

            var rfc2898 = new Rfc2898DeriveBytes(pw, salt, 10000);

            return Convert.ToBase64String(rfc2898.GetBytes(256));
        }
    }
}
