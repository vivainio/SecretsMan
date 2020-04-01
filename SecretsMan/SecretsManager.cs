using System;
using System.Buffers.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SecretsMan
{
    public static class CryptUtil
    {
        public const int KeyLengthBytes = 32;
        public const int IVLengthBytes = 16;
        public const string MagicPrefix = "=K=";
        public const string KeySeparator = "/";
        
        private static Aes GetAes(byte[] key, byte[] iv)
        {
            var aesAlg = Aes.Create() ?? throw new ArgumentNullException("Aes.Create()");
            aesAlg.Key = key;
            aesAlg.IV = iv;
            //aesAlg.Padding = PaddingMode.Zeros;
            return aesAlg;

        }

        public static byte[] CreateIVByRepeatingSalt(byte[] salt)
        {
            var repsNeeded = (IVLengthBytes / salt.Length) + 1;
            var ms = new MemoryStream();
            for (var pos = 0; pos <= IVLengthBytes; pos+=salt.Length)
            {
                ms.Write(salt,0, salt.Length);
            }

            ms.SetLength(IVLengthBytes);
            return ms.ToArray();
        }
        public static byte[] EncryptBytes(byte[] key, byte[] iv, byte[] plainText)
        {
            var aesAlg = GetAes(key, iv);
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            return PerformCryptography(plainText, encryptor);
        }

        public static byte[] DecryptBytes(byte[] key, byte[] iv, byte[] cipherText)
        {
            var aesAlg = GetAes(key, iv);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            return PerformCryptography(cipherText, decryptor);
        }
         
        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return ms.ToArray();
        }

        public static string CreateEncryptedString(string keyId, byte[] content)
        {
            var sb = new StringBuilder();
            sb.Append(MagicPrefix);
            sb.Append(keyId);
            sb.Append(KeySeparator);
            sb.Append(Convert.ToBase64String(content));
            return sb.ToString();
        }

        public static (string KeyName, byte[] Bytes)? ParseEncryptedString(string keyString)
        {
            var sep = keyString.Split(new[] { KeySeparator }, 2, StringSplitOptions.None);
            var head = sep[0];
            if (!head.StartsWith(MagicPrefix) || sep.Length < 2)
            {
                return null;
            }

            var keyName = head.Substring(MagicPrefix.Length);
            var cypherText = Convert.FromBase64String(sep[1]);
            return (keyName, cypherText);
        }
        public static byte[] CreateRandomBytes()
        {
            var bytes = new byte[CryptUtil.KeyLengthBytes];
            var rnd = new RNGCryptoServiceProvider();
            rnd.GetNonZeroBytes(bytes);
            return bytes;
        }
        
    }
    public class SecretsManager
    {
        public static Secrets LoadFromJson(string fname)
        {
            var cont = File.ReadAllBytes(fname);
            var ret = JsonSerializer.Deserialize<Secrets>(cont);
            return ret;
        }

        public static void WriteToJson(string fname, Secrets secrets)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(secrets);
            File.WriteAllBytes(fname, bytes);
        }

        public static void CreateRandomKeyRing()
        {
            var secrets = new Secrets();
        }
    }
}