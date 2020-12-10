using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace SecretsMan
{
    public class JwtSigner
    {

        public static SecurityKey ImportKeyFromJwk(JwkData jwk)
        {
            var rsa = RSA.Create();
            byte[] conv(string val) => val == null ? null : Base64UrlEncoder.DecodeBytes(val);
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = conv(jwk.N),
                Exponent = conv(jwk.E),
                D = conv(jwk.D),
                P =  conv(jwk.P),
                Q = conv(jwk.Q),
                DP = conv(jwk.Dp),
                DQ = conv(jwk.Dq),
                InverseQ = conv(jwk.Qi),
            });
            var key = new RsaSecurityKey(rsa);
            key.KeyId = jwk.Kid;
            return key;
        }
        
        public static string CalculateJwtToken(SecurityKey key, Action<SecurityTokenDescriptor> visitDescriptor)
        {
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature),
            };
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string stoken = jwtSecurityTokenHandler.WriteToken(token);
            return stoken;
        }
    }
}