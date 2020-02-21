namespace Pinger.Models
{
    using System;
    using System.Net;
    using System.Net.NetworkInformation;

    public class PingResult
    {
        public PingResult(long roundTripTime, DateTime requestTime, TimeSpan duration, IPAddress gatewayAddress, IPStatus status)
        {
            this.RoundTripTime = roundTripTime;
            this.RequestTime = requestTime;
            this.Duration = duration;
            this.GatewayAddress = gatewayAddress;
            this.Status = status;
        }

        public DateTime RequestTime { get; }

        public TimeSpan Duration { get; }

        public IPAddress GatewayAddress { get; }

        public long RoundTripTime { get; }

        public IPStatus Status { get; }

        public bool IsSuccess => this.Status == IPStatus.Success;
    }
}
