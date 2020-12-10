using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace SecretsMan.Aws
{
    class Program
    {
        public class TokenData
        {
            public List<Claim> Claims { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
        private static SecurityKey GetKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    RSAParameters rsaKeyInfo = rsa.ExportParameters(true);
                    var key = new RsaSecurityKey(rsaKeyInfo);
                    return key;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }        
        public static string GenerateToken(TokenData data)
        {
            if (data == null || data.Claims == null || !data.Claims.Any())
                throw new ArgumentException("Arguments to create token are not valid.");

            IdentityModelEventSource.ShowPII = true;

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(data.Claims),
                Expires = data.ExpiresAt,
                SigningCredentials = new SigningCredentials(GetKey(), SecurityAlgorithms.RsaSha256Signature),
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jweAymmetric = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            string token = jwtSecurityTokenHandler.WriteToken(jweAymmetric);

            return token;
        }
        static void Main(string[] args)
        {
            var m = new TokenData
            {
                Claims = new List<Claim>()
                {
                    new Claim("a", "b")
                },
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };
            var t = GenerateToken(m);
            Console.WriteLine(t.ToString());
            
        }
    }
}
