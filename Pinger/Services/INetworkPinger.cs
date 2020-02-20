using Pinger.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Pinger.Services
{
    public interface INetworkPinger
    {
        IPAddress Gateway { get; }
        bool IsPingerRunning { get; }
        Task PingerTask { get; }

        event EventHandler<PingerResultEventArgs> Response;

        void Start();
        void Stop();
    }
}