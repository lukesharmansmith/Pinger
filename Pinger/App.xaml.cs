namespace Pinger
{
    using Pinger.Infrastructure;
    using Pinger.Services;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly PingerCoreService pingerCoreService;

        public App()
        {
            this.pingerCoreService = BootStrapper.Instance.Get<PingerCoreService>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Allow app to run headless
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            this.pingerCoreService.Start();

            base.OnStartup(e);
        }
    }
}
