using Pinger.Models;
using System;

namespace Pinger.Services
{
    public interface ITrayIconHandler
    {
        event EventHandler TrayIconClicked;

        void SetTrayIconState(NetworkUpState state);
    }
}