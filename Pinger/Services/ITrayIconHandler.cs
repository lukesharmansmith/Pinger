using Pinger.Models;
using System;

namespace Pinger.Services
{
    public interface ITrayIconHandler
    {
        event EventHandler LiveViewClicked;
        event EventHandler ClosedApplicationClicked;
        event EventHandler TestToastClicked;

        void SetTrayIconState(NetworkUpState state);
    }
}