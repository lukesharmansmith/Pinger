namespace Pinger.ViewModels
{
    using Pinger.Infrastructure;
    using Pinger.Models;
    using Pinger.Services;
    using Prism.Commands;
    using Prism.Mvvm;
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class LivePingerViewModel : BindableBase, ILivePingerViewModel
    {
        private readonly INetworkPinger pinger;

        private string address;

        private string reply;

        private long roundTripTime;

        private TimeSpan requestTime;

        private double duration;

        public LivePingerViewModel()
          :  this(BootStrapper.Instance.Get<INetworkPinger>())
        {
            // Work around a potential netcore issue where application wide resource cannot be resolved when binding to datacontext  
            // https://github.com/dotnet/wpf/issues/2543
        }

        public LivePingerViewModel(INetworkPinger pinger)
        {
            this.pinger = pinger;

            this.LoadedCommand = new DelegateCommand(() =>
            {
                this.pinger.Response += this.OnPingerResponse;
            });

            this.UnLoadedCommand = new DelegateCommand(() =>
            {
                this.pinger.Response -= this.OnPingerResponse;
            });
        }

        public ICommand LoadedCommand { get; }

        public ICommand UnLoadedCommand { get; }

        public string Address
        {
            get => this.address;
            set
            {
                this.address = value;
                this.RaisePropertyChanged();
            }
        }

        public string Reply
        {
            get => this.reply;
            set
            {
                this.reply = value;
                this.RaisePropertyChanged();
            }
        }

        public long RoundTripTime
        {
            get => this.roundTripTime;
            set
            {
                this.roundTripTime = value;
                this.RaisePropertyChanged();
            }
        }

        public TimeSpan RequestTime
        {
            get => this.requestTime;
            set
            {
                this.requestTime = value;
                this.RaisePropertyChanged();
            }
        }

        public double Duration
        {
            get => this.duration;
            set
            {
                this.duration = value;
                this.RaisePropertyChanged();
            }
        }

        private void OnPingerResponse(object sender, PingerResultEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.Address = e.Result.GatewayAddress.ToString();
                this.Reply = e.Result.Status.ToString();
                this.RoundTripTime = e.Result.RoundTripTime;
                this.RequestTime = e.Result.RequestTime.TimeOfDay;
                this.Duration = e.Result.Duration.TotalMilliseconds;
            });
        }
    }
}
