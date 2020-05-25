using System;
using System.Collections.Generic;

namespace SecretsMan
{
    // representation in json
    public class Key
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string? Expires { get; set; }
        public string Issued { get; set; }
        public byte[]? Decrypted { get; set; }

        // yeah, salt is based on key NAME
        public byte[] IvSalt => CryptUtil.CreateIVByRepeatingSalt(Name.ToUtf8());

        public static Key CreateFromBytes(string name, byte[] content) =>
            new Key()
            {
                Name = name,
                Decrypted = content
            };
    }

    public class Secrets
    {
        public string? EncryptedWithKey { get; set; }

        private Dictionary<string, byte[]> DecryptedKeys { get; set; } = new Dictionary<string, byte[]>();

        //public string? CheckSum { get; set; }
        public List<Key> Keys { get; set; } = new List<Key>();

        public void AddKeys(Secrets moreKeys)
        {
            foreach (var it in moreKeys.DecryptedKeys)
            {
                DecryptedKeys.Add(it.Key, it.Value);
            }

            Keys.AddRange(moreKeys.Keys);
        }

        public static Key IssueKey(string name, Key masterKey)
        {
            var key = Key.CreateFromBytes(name, CryptUtil.CreateRandomBytesForKey());
            key.Issued = DateTime.UtcNow.ToString("o");
            
            var encryptedWithMaster = CryptUtil.EncryptBytes(masterKey.Decrypted, key.IvSalt, key.Decrypted);
            key.Text = CryptUtil.CreateEncryptedString(masterKey.Name, encryptedWithMaster);
            return key;
        }
    }
}