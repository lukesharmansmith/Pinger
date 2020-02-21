namespace Pinger.Services
{
    using System.Windows;

    public class WindowHandler : IWindowHandler
    {
        public void ShowLiveWindow()
        {
            // Make sure we have a window open (in case user clicked toast while app closed)
            if (App.Current.Windows.Count == 0)
            {
                new LivePingerView().Show();
            }

            // Activate the window, bringing it to focus
            App.Current.Windows[0].Activate();

            // And make sure to maximize the window too, in case it was currently minimized
            App.Current.Windows[0].WindowState = WindowState.Normal;
        }

        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
