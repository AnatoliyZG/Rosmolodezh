using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RosMolServer
{
    internal static class HashManager
    {

        public static string GenerateMD5Hash(string value)
        {
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);

            using var hash = MD5.Create();
            
            return Convert.ToHexString(hash.ComputeHash(valueBytes));
        }
    }
}
