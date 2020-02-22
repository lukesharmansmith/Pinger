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

            this.notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            this.notifyIcon.ContextMenuStrip.Items.Add("Live view", null, (sender, args) => this.LiveViewClicked?.Invoke(this, EventArgs.Empty));
            this.notifyIcon.ContextMenuStrip.Items.Add("Raise test toast", null, (sender, args) => this.TestToastClicked?.Invoke(this, EventArgs.Empty));
            this.notifyIcon.ContextMenuStrip.Items.Add("Close", null, (sender, args) => this.ClosedApplicationClicked?.Invoke(this, EventArgs.Empty));

            this.notifyIcon.Click += (sender, args) => this.notifyIcon.ContextMenuStrip.Show(Cursor.Position);
        }

        public event EventHandler LiveViewClicked;

        public event EventHandler ClosedApplicationClicked;

        public event EventHandler TestToastClicked;

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
