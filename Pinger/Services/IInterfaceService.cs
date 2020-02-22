namespace Pinger.Services
{
    public interface IInterfaceService
    {
        IToastService ToastService { get; }
        ITrayIconHandler TrayIconHandler { get; }
        IWindowHandler WindowHandler { get; }
    }
}