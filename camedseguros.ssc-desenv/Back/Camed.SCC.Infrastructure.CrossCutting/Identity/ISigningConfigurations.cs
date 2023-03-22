using Microsoft.IdentityModel.Tokens;

namespace Camed.SCC.Infrastructure.CrossCutting.Identity
{
    public interface ISigningConfigurations
    {
        SecurityKey Key { get; }
        SigningCredentials SigningCredentials { get; }
    }
}
