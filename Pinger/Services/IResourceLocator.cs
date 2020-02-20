using Pinger.Models;

namespace Pinger.Services
{
    public interface IResourceLocator
    {
        string GetPathToAsset(ResourceAsset asset);
    }
}