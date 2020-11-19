using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelperMethods.Helper
{
    public static class Cryptographie
    {
        public static string GenerateUniqueToken()
        {
            byte[] random = new byte[100];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            using (SHA512 shaM = new SHA512Managed())
            {
                var hash = shaM.ComputeHash(random);                
                return GetHashString(hash);
            }
        }

        static string GetHashString(byte[] input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in input)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
