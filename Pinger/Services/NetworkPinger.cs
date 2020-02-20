namespace Pinger.Services
{
    using Pinger.Models;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class NetworkPinger : INetworkPinger
    {
        private readonly Ping pingSender = new Ping();

        private readonly PingOptions options = new PingOptions();

        private readonly int timeout = 30;

        private TimeSpan delay = TimeSpan.FromSeconds(1);

        private CancellationTokenSource pingerTaskCancellationSource;

        public NetworkPinger()
        {
            this.Gateway = this.GetDefaultGateway();
        }

        public event EventHandler<PingerResultEventArgs> Response;

        public IPAddress Gateway { get; private set; }

        public Task PingerTask { get; private set; }

        public bool IsPingerRunning => this.PingerTask != null && this.PingerTask.Status != TaskStatus.RanToCompletion;

        public void Start()
        {
            if (this.IsPingerRunning)
            {
                Stop();
            }

            this.pingerTaskCancellationSource = new CancellationTokenSource();
            this.PingerTask = Task.Run(async () => await this.PingerRunLoop(this.pingerTaskCancellationSource.Token));
        }

        public void Stop()
        {
            if (this.pingerTaskCancellationSource != null)
            {
                this.pingerTaskCancellationSource.Cancel();
            }
        }

        private static byte[] GetTestPacketData()
        {
            const string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            return Encoding.ASCII.GetBytes(data);
        }

        private async Task PingerRunLoop(CancellationToken token)
        {
            this.Gateway = this.GetDefaultGateway();
            var bufferData = GetTestPacketData();

            try
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var requestTimestamp = DateTime.Now;
                        var reply = pingSender.Send(this.Gateway, timeout, bufferData, options);
                        var recvTimeDuration = DateTime.Now - requestTimestamp;

                        this.Response?.Invoke(this, new PingerResultEventArgs(new PingResult(reply, requestTimestamp, recvTimeDuration, this.Gateway)));
                    }
                    catch (Exception ex)
                    {
                        // TODO handle
                    }

                    await Task.Delay(this.delay, token);
                }
            }
            catch (TaskCanceledException)
            {
                // Cancelled
            }
        }

        private IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .FirstOrDefault();
        }
    }
}
