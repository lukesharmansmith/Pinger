using Pinger.Models;

namespace Pinger.Services
{
    public interface IToastService
    {
        void Show(PingResult result);
        void Show(string message);
    }
}