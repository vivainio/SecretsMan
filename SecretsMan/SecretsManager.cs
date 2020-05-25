using System.Buffers.Text;
using System.IO;
using System.Text.Json;

namespace SecretsMan
{
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