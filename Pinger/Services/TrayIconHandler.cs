namespace Pinger.Services
{
    using Pinger.Models;
    using System;
    using System.Windows.Forms;

    public class TrayIconHandler : ITrayIconHandler
    {
        private readonly NotifyIcon notifyIcon;

        public TrayIconHandler()
        {
            this.notifyIcon = new NotifyIcon
            {
                Visible = true,
                Text = "Pinger - For when powerlines fail.."
            };

            this.SetTrayIconState(NetworkUpState.Normal);

            this.notifyIcon.Click += (sender, args) => this.TrayIconClicked?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TrayIconClicked;

        public void SetTrayIconState(NetworkUpState state)
        {
            switch (state)
            {
                case NetworkUpState.Failed:
                    this.notifyIcon.Icon = Properties.Resources.TrayIconBad;
                    break;
                default:
                    this.notifyIcon.Icon = Properties.Resources.TrayIconNormal;
                    break;
            }
        }
    }
}
