namespace Pinger.Services
{
    using Pinger.Models;
    using System;
    using System.IO;

    public class ResourceLocator : IResourceLocator
    {
        private readonly string basePath;

        public ResourceLocator()
        {
            this.basePath = AppDomain.CurrentDomain.BaseDirectory;
        }

        public string GetPathToAsset(ResourceAsset asset)
        {
            switch (asset)
            {
                case ResourceAsset.ToastIconNormal:
                    return Path.Combine(this.basePath, "IconNormalToast.png");
                case ResourceAsset.ToastIconBad:
                    return Path.Combine(this.basePath, "IconBadToast.png");
                default:
                    throw new ArgumentOutOfRangeException(nameof(asset), $"Cannot find asset for type '{asset}' ");
            }
        }
    }
}
