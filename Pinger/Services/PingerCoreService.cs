namespace Pinger.Services
{
    using Pinger.Models;
    using System;
    using System.Linq;

    public class PingerCoreService
    {
        private readonly IInterfaceService interfaceService;

        private readonly INetworkPinger networkPinger;

        private readonly RingBuffer<PingResult> sampleBuffer;

        private DateTime lastNotifyTime;

        private DateTime lastSampleTime;

        public PingerCoreService(IInterfaceService interfaceService, INetworkPinger networkPinger)
        {
            this.interfaceService = interfaceService;
            this.networkPinger = networkPinger;

            this.NotifyBackOff = TimeSpan.FromSeconds(15);
            this.SampleTimingThreshold = TimeSpan.FromSeconds(5);
            this.MaxSamples = 5;
            this.sampleBuffer = new RingBuffer<PingResult>(this.MaxSamples);

            this.networkPinger.Response += this.OnPingerResponse;
        }

        public TimeSpan SampleTimingThreshold { get; set; }

        public TimeSpan NotifyBackOff { get; set; }

        public int MaxSamples { get; set; }

        public void Start()
        {
            this.interfaceService.ToastService.Show("Pinger starting...");
            this.networkPinger.Start();
        }

        private void OnPingerResponse(object sender, PingerResultEventArgs e)
        {
            this.sampleBuffer.AddItemToQueue(e.Result);
            this.CheckBuffer();
        }

        private void CheckBuffer()
        {
            var currentTime = DateTime.Now;

            // Only sample the buffer after X number of seconds
            if ((currentTime - this.lastSampleTime) > this.SampleTimingThreshold)
            {
                var multipleFailures = this.sampleBuffer.Get.All(x => !x.IsSuccess);

                // Dont over saturate the failure messages
                if (multipleFailures && currentTime > this.lastNotifyTime.Add(NotifyBackOff))
                {
                    this.lastNotifyTime = currentTime;

                    this.interfaceService.ToastService.Show(new ToastMessage
                    {
                        Title = "Multiple Bad Pings",
                        Line1 = $"{MaxSamples} bad responses attempting to ping {this.networkPinger.Gateway.ToString()}",
                        Icon = ResourceAsset.ToastIconBad
                    });
                }

                this.interfaceService.TrayIconHandler.SetTrayIconState(multipleFailures ? NetworkUpState.Failed : NetworkUpState.Normal);

                this.lastSampleTime = currentTime;
            }
        }
    }
}
