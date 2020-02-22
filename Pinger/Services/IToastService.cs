namespace Pinger.Services
{
    using Pinger.Models;

    public interface IToastService
    {
        void Show(ToastMessage message);
        void Show(string message);
    }
}