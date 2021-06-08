using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Volvo.Ecash.Domain.Entities;

namespace Volvo.Ecash.Infrastructure.Seed
{
    public static class ModelBuilderExtensions
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
                stringBuilder.Append(String.Format("{0:x2}", item));
            return stringBuilder.ToString();
        }

    }
}
