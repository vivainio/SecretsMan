using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SecretsMan.Jwt;

namespace SecretsMan.Aws
{
    public class TokenRequest
    {
        public List<Claim> Claims { get; set; }
        public DateTime ExpiresAt { get; set; }
        public SecurityKey Key { get; set; }
    }

}
