namespace Pinger.Services
{
    using Pinger.Models;
    using System;
    using System.Linq;

    public class PingerCoreService
    {
        private readonly ITrayIconHandler trayIconHandler;

        private readonly IToastService toastService;

        private readonly INetworkPinger networkPinger;

        private readonly IWindowHandler windowHandler;

        private readonly RingBuffer<PingResult> sampleBuffer;

        private DateTime lastNotifyTime;

        private DateTime lastSampleTime;

        public PingerCoreService(ITrayIconHandler trayIconHandler, IToastService toastService, INetworkPinger networkPinger, IWindowHandler windowHandler)
        {
            this.trayIconHandler = trayIconHandler;
            this.toastService = toastService;
            this.networkPinger = networkPinger;
            this.windowHandler = windowHandler;

            this.NotifyBackOff = TimeSpan.FromSeconds(15);
            this.SampleTimingThreshold = TimeSpan.FromSeconds(5);
            this.MaxSamples = 5;
            this.sampleBuffer = new RingBuffer<PingResult>(this.MaxSamples);

            this.networkPinger.Response += this.OnPingerResponse;
            this.trayIconHandler.TrayIconClicked += (sender, arg) => windowHandler.ShowLiveWindow();
        }

        public TimeSpan SampleTimingThreshold { get; set; }

        public TimeSpan NotifyBackOff { get; set; }

        public int MaxSamples { get; set; }

        public void Start()
        {
            this.toastService.Show("Pinger starting...");
            this.networkPinger.Start();
        }

        private void OnPingerResponse(object sender, PingerResultEventArgs e)
        {
            this.sampleBuffer.AddItemToQueue(e.Result);
            this.CheckBuffer(e.Result);
        }

        private void CheckBuffer(PingResult latestResult)
        {
            var currentTime = DateTime.Now;

            // Only sample the buffer after X number of seconds
            if ((currentTime - this.lastSampleTime) > this.SampleTimingThreshold)
            {
                var success = this.sampleBuffer.Get.All(x => x.IsSuccess);

                // Dont over saturate the failure messages
                if (!success && currentTime > this.lastNotifyTime.Add(NotifyBackOff))
                {
                    this.lastNotifyTime = currentTime;

                    this.toastService.Show(latestResult);
                }

                this.trayIconHandler.SetTrayIconState(success ? NetworkUpState.Normal : NetworkUpState.Failed);

                this.lastSampleTime = currentTime;
            }
        }
    }
}
