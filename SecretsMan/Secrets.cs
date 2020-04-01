using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SecretsMan
{
    
    // representation in json
    public class Key
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string? Expires { get; set; }
        public string? Issued { get; set; }
        private byte[] Decrypted { get; set; }

    }
    public class Secrets
    {
        
        public string? EncryptedWithKey { get; set; }
        private Dictionary<string, byte[]> DecryptedKeys { get; set; } = new Dictionary<string, byte[]>();
        //public string? CheckSum { get; set; }
        public List<Key> Keys { get; set;} = new List<Key>();
        
        
        
        public void AddKeys(Secrets moreKeys)
        {
            foreach (var it in moreKeys.DecryptedKeys)
            {
                DecryptedKeys.Add(it.Key, it.Value);
            }
            
            Keys.AddRange(moreKeys.Keys);
        }
        
        
    }
}
