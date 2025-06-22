using System.Security.Cryptography;

namespace Core
{
    internal class Hehe : PasswordDeriveBytes
    {
        internal Hehe(string strPassword, byte[] rgbSalt) : base(strPassword, rgbSalt) { }
    }
}
