using System;
using System.Linq;

namespace Ecubytes.Identity.User.Api.Utils
{
    public sealed class CryptoPassword
    {
        public static string EncryptPassword(string logonName, string password)
        {
            return Cryptography.CryptoHelper.GetHashString($"{logonName}@#{password}");
        }
    }
}
