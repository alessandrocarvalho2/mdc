using System;
using System.Security.Cryptography;
using System.Text;

namespace Volvo.Ecash.Application.Utils
{
    public static class Encryption
    {

        public static string Encoding(string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] HashValue, MessageBytes = UE.GetBytes(value);

            using (SHA256Managed SHhash = new SHA256Managed())
            {
                HashValue = SHhash.ComputeHash(MessageBytes);
            }

            foreach (byte item in HashValue)
            {

                stringBuilder.Append(string.Format("{0:x2}", item));
            }
            return stringBuilder.ToString();
        }


    }
}
