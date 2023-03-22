using Camed.SCC.Infrastructure.CrossCutting.Identity;

namespace Camed.SSC.Application.Interfaces
{
    public interface IIdentityAppService
    {
        SSCIdentity Identity { get; }
    }
}
