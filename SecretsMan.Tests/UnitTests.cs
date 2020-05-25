﻿using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoFixture;
using NFluent;
using TrivialTestRunner;

namespace SecretsMan.Tests
{
    public class UnitTests
    {
        private Fixture fixture;

        public UnitTests()
        {
            this.fixture = new Fixture();
        }

        private byte[] SomeBytes(int i) => fixture.CreateMany<byte>(i).ToArray();
        
        [Case]
        public void DirectEncrypt()
        {
            var plainText = Encoding.UTF8.GetBytes("Hello world");
            var iv = SomeBytes(16);
            var key = SomeBytes(32);
            var encrypted = CryptUtil.EncryptBytes(key, iv, plainText);
            var decrypted = CryptUtil.DecryptBytes(key, iv, encrypted);
            Check.That(decrypted).Equals(plainText);
        }

        [Case]
        public void TestCryptedtrings()
        {
            var b = SomeBytes(20);
            var k = CryptUtil.CreateEncryptedString("mykey", b);
            var parsed = CryptUtil.ParseEncryptedString(k);
            Check.That(parsed?.Bytes).Equals(b);
            Check.That(parsed?.KeyName == "mykey");
        }
        
        [Case]
        public void TestInitSecrets()
        {
            var secrets = fixture.Create<Secrets>();
            SecretsManager.WriteToJson("secrets.json", secrets);
        }

        [Case]
        public void TestSalt()
        {
            var saltbytes = CryptUtil.CreateIVByRepeatingSalt("re!".ToUtf8());
            var decoded = saltbytes.Decode();
            Check.That(decoded).Equals("re!re!re!re!re!r");
        }

        [Case]
        public void IssueNewKey()
        {
            var masterkey = Key.CreateFromBytes("masterkey", CryptUtil.CreateRandomBytesForKey());

            var keyRing = new Secrets();
            keyRing.Keys.Add(Secrets.IssueKey("user_key_1", masterkey));
            keyRing.Keys.Add( Secrets.IssueKey("user_key_2", masterkey));
            
        }
        
    }
}