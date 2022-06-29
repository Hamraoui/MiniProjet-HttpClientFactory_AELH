using System.Security.Cryptography;
using System.Text;

namespace myApiV1._0.Tools
{
    public class Password
    {
        // function to hash password

        public static string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asBytesArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asBytesArray);

            return Convert.ToBase64String(hashedPassword);
        }
    }
}
