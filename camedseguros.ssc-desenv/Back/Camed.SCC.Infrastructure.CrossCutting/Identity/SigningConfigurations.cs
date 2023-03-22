﻿using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Camed.SCC.Infrastructure.CrossCutting.Identity
{
    public class SigningConfigurations : ISigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
