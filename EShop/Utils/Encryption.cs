using System;
using System.Security.Cryptography;
using System.Text;

namespace EShop.Utils
{
    public class Encryption
    {
        public static string SHA1(string text)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] sha1data = sha1.ComputeHash(Encoding.ASCII.GetBytes(text));
            return Convert.ToBase64String(sha1data);
        }

        public static string SHA256(string text)
        {
            var mySHA256 = System.Security.Cryptography.SHA256.Create();
            byte[] sha256Data = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(text));
            return Convert.ToBase64String(sha256Data);
        }
    }
}