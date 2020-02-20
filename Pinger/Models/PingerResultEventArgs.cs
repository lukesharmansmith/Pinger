namespace Pinger.Models
{
    using System;

    public class PingerResultEventArgs : EventArgs
    {
        public PingerResultEventArgs(PingResult result)
        {
            this.Result = result;
        }

        public PingResult Result { get; }
    }
}
