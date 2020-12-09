using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RoomMate.Common
{
    public static class Crypto
    {
        public static string CreateMD5(string value)
        {
            return Convert.ToBase64String(
                SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }
}