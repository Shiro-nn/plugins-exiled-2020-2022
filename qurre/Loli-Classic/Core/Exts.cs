using HarmonyLib;
using Qurre.API;
using Qurre.API.Attributes;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Core
{
    static class Exts
    {
        static internal string Combine(string path1, string path2, string path3, string path4)
            => Path.Combine(path1, path2, path3, path4);

        static internal bool Exists(string path)
            => File.Exists(path);

        static internal Assembly Load(byte[] bytes)
            => Assembly.Load(bytes);

        static internal byte[] ReadAllBytes(string path)
            => File.ReadAllBytes(path);

        static internal Type TypeByName(string name)
            => AccessTools.TypeByName(name);

        static internal MethodInfo GetMethod(Type type, string name, Type[] parameters = null, Type[] generics = null)
            => AccessTools.Method(type, name, parameters, generics);

        static internal string Plugins
            => Pathes.AppData;

        static internal byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
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

        static internal byte[] Decrypt(byte[] cipherData, string Password)
        {
            Hehe pdb = new(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        static internal void Enable(System.Reflection.Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var attr = type.GetCustomAttribute<PluginInit>();
                if (attr is null)
                    continue;

                foreach (var methodInfo in type
                    .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (methodInfo.IsAbstract)
                    {
                        continue;
                    }
                    if (!methodInfo.IsStatic)
                    {
                        continue;
                    }

                    if (methodInfo.GetCustomAttribute<PluginEnable>() is not null)
                        methodInfo.Invoke(null, new object[] { });
                }
            }
        }
    }
}