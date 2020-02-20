namespace Pinger.Models
{
    using System;
    using System.Net;
    using System.Net.NetworkInformation;

    public class PingResult
    {
        public PingResult(PingReply pingReply, DateTime requestTime, TimeSpan duration, IPAddress gatewayAddress)
        {
            this.PingReply = pingReply;
            this.RequestTime = requestTime;
            this.Duration = duration;
            this.GatewayAddress = gatewayAddress;
        }

        public PingReply PingReply { get; }

        public DateTime RequestTime { get; }

        public TimeSpan Duration { get; }

        public IPAddress GatewayAddress { get; }

        public bool IsSuccess => this.PingReply?.Status == IPStatus.Success;
    }
}
