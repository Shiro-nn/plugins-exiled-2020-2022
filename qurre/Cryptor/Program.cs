using System.Security.Cryptography;
using System.Text;

namespace Cryptor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dir = args[0];
            string key = args[1];

            string name = args[2] ?? string.Join('.', dir.Split('\\')[^1].Split('.').SkipLast(1));

            Console.WriteLine(name);
            Console.WriteLine(string.Join('\\', dir.Split('\\').SkipLast(1)));

            Encrypt(dir, string.Join('\\', dir.Split('\\').SkipLast(1)) + "\\" + name + ".bin", key);
        }
        // Encrypt a byte array into a byte array using a key and an IV 
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new();
            Aes alg = Aes.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(clearData, 0, clearData.Length);

            cs.Close();

            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        // Encrypt a string into a string using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 

        public static string Encrypt(string clearText, string Password)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            PasswordDeriveBytes pdb = new(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            byte[] encryptedData = Encrypt(clearBytes,
                     pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);

        }

        // Encrypt bytes into bytes using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 

        public static byte[] Encrypt(byte[] clearData, string Password)
        {
            PasswordDeriveBytes pdb = new(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));

        }

        public static void Encrypt(string fileIn, string fileOut, string Password)
        {
            FileStream fsIn = new(fileIn,
                FileMode.Open, FileAccess.Read);
            FileStream fsOut = new(fileOut,
                FileMode.OpenOrCreate, FileAccess.Write);

            PasswordDeriveBytes pdb = new(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            Aes alg = Aes.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);

            CryptoStream cs = new (fsOut, alg.CreateEncryptor(), CryptoStreamMode.Write);

            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;

            do
            {
                bytesRead = fsIn.Read(buffer, 0, bufferLen);

                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);

            cs.Close();
            fsIn.Close();
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new();

            Aes alg = Aes.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(cipherData, 0, cipherData.Length);

            cs.Close();

            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

        // Decrypt a string into a string using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 

        public static string Decrypt(string cipherText, string Password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes pdb = new(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65,
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            byte[] decryptedData = Decrypt(cipherBytes,
                pdb.GetBytes(32), pdb.GetBytes(16));

            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        // Decrypt bytes into bytes using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 

        public static byte[] Decrypt(byte[] cipherData, string Password)
        {
            PasswordDeriveBytes pdb = new(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        // Decrypt a file into another file using a password 
        public static void Decrypt(string fileIn,
                    string fileOut, string Password)
        {
            FileStream fsIn = new(fileIn, FileMode.Open, FileAccess.Read);
            FileStream fsOut = new(fileOut, FileMode.OpenOrCreate, FileAccess.Write);

            PasswordDeriveBytes pdb = new(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            Aes alg = Aes.Create();

            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);

            CryptoStream cs = new(fsOut, alg.CreateDecryptor(), CryptoStreamMode.Write);

            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;

            do
            {
                bytesRead = fsIn.Read(buffer, 0, bufferLen);

                cs.Write(buffer, 0, bytesRead);

            } while (bytesRead != 0);

            cs.Close();
            fsIn.Close();
        }
    }
}