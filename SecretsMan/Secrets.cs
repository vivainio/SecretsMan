using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SecretsMan
{
    public class Secrets
    {
        
        public string? EncryptedWithKey { get; set; }
        public Dictionary<string, string> Keys { get; set; } = new Dictionary<string, string>();
        //public string? CheckSum { get; set; }

        public void AddKeys(Secrets moreKeys)
        {
            foreach (var it in moreKeys.Keys)
            {
                Keys.Add(it.Key, it.Value);
            }

        }
    }
}
