using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManageStudent.PasswordHandle
{
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordHandle
    {
        private static PasswordHandle _instance;
        public static PasswordHandle GetInstance()
        {
            return _instance ?? (_instance = new PasswordHandle());
        }

        public string GenerateSalt()
        {
            return Guid.NewGuid().ToString();
        }

        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string EncryptPassword(string originalPassword, string salt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(originalPassword + salt))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
