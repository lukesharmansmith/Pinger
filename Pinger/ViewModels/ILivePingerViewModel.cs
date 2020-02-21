namespace Pinger.ViewModels
{
    using System;
    using System.Windows.Input;

    public interface ILivePingerViewModel
    {
        string Address { get; set; }
        double Duration { get; set; }
        ICommand ExitCommand { get; }
        ICommand LoadedCommand { get; }
        string Reply { get; set; }
        TimeSpan RequestTime { get; set; }
        long RoundTripTime { get; set; }
        ICommand UnLoadedCommand { get; }
    }
}