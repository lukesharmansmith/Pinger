namespace Pinger.Services
{
    using Pinger.Models;

    public class InterfaceService : IInterfaceService
    {
        public InterfaceService(IWindowHandler windowHandler, IToastService toastService, ITrayIconHandler trayIconHandler)
        {
            this.WindowHandler = windowHandler;
            this.ToastService = toastService;
            this.TrayIconHandler = trayIconHandler;

            this.TrayIconHandler.LiveViewClicked += (sender, arg) => windowHandler.ShowLiveWindow();
            this.TrayIconHandler.ClosedApplicationClicked += (sender, arg) => windowHandler.ExitApplication();
            this.TrayIconHandler.TestToastClicked += (sender, arg) =>
            {
                this.ToastService.Show(new ToastMessage
                {
                    Title = "Test toast",
                    Line1 = "This is some text on line 1",
                    Line2 = "This is some text on line 2",
                    Icon = ResourceAsset.ToastIconBad
                });
            };
        }

        public ITrayIconHandler TrayIconHandler { get; }

        public IWindowHandler WindowHandler { get; }

        public IToastService ToastService { get; }
    }
}