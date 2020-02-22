namespace Pinger.Infrastructure
{
    using Ninject.Modules;
    using Pinger.Services;
    using Pinger.ViewModels;

    public class PingerModule : NinjectModule
    {
        public override void Load()
        {
            // Services
            Bind<INetworkPinger>().To<NetworkPinger>().InSingletonScope();
            Bind<IResourceLocator>().To<ResourceLocator>().InSingletonScope();
            Bind<IToastService>().To<ToastService>().InSingletonScope();
            Bind<ITrayIconHandler>().To<TrayIconHandler>().InSingletonScope();
            Bind<IWindowHandler>().To<WindowHandler>().InSingletonScope();
            Bind<IInterfaceService>().To<InterfaceService>().InSingletonScope();

            Bind<PingerCoreService>().ToSelf().InSingletonScope();

            // Views
            Bind<ILivePingerViewModel>().To<LivePingerViewModel>();
        }
    }
}
